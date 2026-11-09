using DosingApp.DataContext;
using DosingApp.Models;
using DosingApp.Models.AgroAssist.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DosingApp.Services.Import
{
    public class RecipeImportService
    {
        private readonly AppDbContext _db;

        public RecipeImportService(AppDbContext db) => _db = db;

        public async Task<int> SaveFromTechOperationsAsync(IEnumerable<TechOperationDto> techOperations, CancellationToken ct = default)
        {
            var list = techOperations
                .Where(t => t.TechoperationGuid != Guid.Empty)
                .GroupBy(t => t.TechoperationGuid)
                .Select(g => g.Last())
                .ToList();

            if (list.Count == 0) return 0;

            // 1. все PlanInventory → entity_guid для поиска Component
            var componentGuids = list
                .SelectMany(t => t.PlanInventories ?? new List<PlanInventoryDto>())
                .Where(p => p.EntityGuid.HasValue && p.EntityGuid.Value != Guid.Empty)
                .Select(p => p.EntityGuid.Value)
                .ToList();

            // 2. Component по PesticideGuid (entity_guid)
            var componentByPesticide = new Dictionary<Guid, Component>();
            const int Chunk = 500;
            for (int i = 0; i < componentGuids.Count; i += Chunk)
            {
                var chunk = componentGuids.Skip(i).Take(Chunk).ToList();
                var rows = await _db.Components
                    .Where(c => c.PesticideGuid.HasValue && chunk.Contains(c.PesticideGuid.Value))
                    .ToListAsync(ct)
                    .ConfigureAwait(false);

                foreach (var c in rows)
                    componentByPesticide[c.PesticideGuid.Value] = c;
            }

            // 3. Существующие рецепты по TechoperationGuid
            var techGuids = list
                .Where(t => t.TechoperationGuid != Guid.Empty)
                .Select(t => t.TechoperationGuid)
                .ToList();

            var existingRecipes = new Dictionary<Guid, Recipe>();
            for (int i = 0; i < techGuids.Count; i += Chunk)
            {
                var chunk = techGuids.Skip(i).Take(Chunk).ToList();
                var rows = await _db.Recipes
                    .Where(r => r.TechOperationGuid.HasValue && chunk.Contains(r.TechOperationGuid.Value))
                    .Include(r => r.RecipeComponents)
                    .ToListAsync(ct)
                    .ConfigureAwait(false);

                foreach (var r in rows)
                    existingRecipes[r.TechOperationGuid.Value] = r;
            }

            var waterComponent = await _db.Components.FirstOrDefaultAsync(c => c.Name == Water.Name, ct).ConfigureAwait(false); ;

            // 4. Строим/обновляем рецепты
            var toInsert = new List<Recipe>();
            var toUpdate = new List<Recipe>();

            foreach (var tech in list)
            {
                var planList = tech.PlanInventories ?? new List<PlanInventoryDto>();

                if (existingRecipes.TryGetValue(tech.TechoperationGuid, out var recipe))
                {
                    UpdateRecipe(recipe, tech, planList, componentByPesticide);
                    toUpdate.Add(recipe);
                }
                else
                {
                    var newRecipe = CreateRecipe(tech, planList, componentByPesticide, waterComponent);
                    toInsert.Add(newRecipe);
                }
            }

            // 5. Сохраняем
            var oldAutoDetect = _db.ChangeTracker.AutoDetectChangesEnabled;
            _db.ChangeTracker.AutoDetectChangesEnabled = false;
            try
            {
                if (toInsert.Count > 0)
                {
                    using (var tx = await _db.Database.BeginTransactionAsync(ct).ConfigureAwait(false))
                    {
                        for (int i = 0; i < toInsert.Count; i += 500)
                        {
                            var batch = toInsert.GetRange(i, Math.Min(500, toInsert.Count - i));
                            await _db.Recipes.AddRangeAsync(batch, ct).ConfigureAwait(false);
                            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
                            ClearTracker();
                        }
                        await tx.CommitAsync(ct).ConfigureAwait(false);
                    }
                }

                if (toUpdate.Count > 0)
                {
                    using (var tx = await _db.Database.BeginTransactionAsync(ct).ConfigureAwait(false))
                    {
                        for (int i = 0; i < toUpdate.Count; i += 500)
                        {
                            var batch = toUpdate.GetRange(i, Math.Min(500, toUpdate.Count - i));
                            _db.Recipes.UpdateRange(batch);
                            await _db.SaveChangesAsync(ct).ConfigureAwait(false);
                            ClearTracker();
                        }
                        await tx.CommitAsync(ct).ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                _db.ChangeTracker.AutoDetectChangesEnabled = oldAutoDetect;
            }

            return list.Count;
        }

        private static Recipe CreateRecipe(
            TechOperationDto tech,
            List<PlanInventoryDto> planList,
            Dictionary<Guid, Component> componentByPesticide,
            Component waterComponent)
        {
            var recipe = new Recipe
            {
                CarrierReserve = 20.0,
                CarrierId = waterComponent.ComponentId,
                TechOperationGuid = tech.TechoperationGuid,
                Name = $"Рецепт (техоперация {tech.SeqId})",
                Note = null,
                IsMotherLiquor = false,
                RecipeComponents = new List<RecipeComponent>(),
            };

            int order = 1;
            foreach (var p in planList)
            {
                recipe.RecipeComponents.Add(BuildRecipeComponent(recipe, p, order++, componentByPesticide));
            }
            return recipe;
        }

        private static void UpdateRecipe(
            Recipe recipe,
            TechOperationDto tech,
            List<PlanInventoryDto> planList,
            Dictionary<Guid, Component> componentByPesticide)
        {
            // Коллекция загружена через Include — EF сравнит и удалит лишних
            recipe.RecipeComponents.Clear();

            int order = 1;
            foreach (var p in planList)
            {
                recipe.RecipeComponents.Add(BuildRecipeComponent(recipe, p, order++, componentByPesticide));
            }
        }

        private static RecipeComponent BuildRecipeComponent(
            Recipe recipe,
            PlanInventoryDto p,
            int order,
            Dictionary<Guid, Component> componentByPesticide)
        {
            int? componentId = null;
            if (p.EntityGuid.HasValue && componentByPesticide.TryGetValue(p.EntityGuid.Value, out var comp))
            {
                componentId = comp.ComponentId;
            }

            double? volumeRate = null;
            if (p.Total.HasValue && p.PlanAreaHectare.HasValue && p.PlanAreaHectare.Value > 0)
            {
                volumeRate = p.Total.Value / p.PlanAreaHectare.Value;
            }

            return new RecipeComponent
            {
                Recipe = recipe,
                ComponentId = componentId,
                Order = order,
                VolumeRate = volumeRate,
                VolumeRateUnit = null,
                Dispenser = null,
            };
        }

        private void ClearTracker()
        {
            foreach (var entry in _db.ChangeTracker.Entries().ToList())
                entry.State = EntityState.Detached;
        }
    }
}
