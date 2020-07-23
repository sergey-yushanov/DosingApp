using DosingApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DosingApp
{
    public class AppDbContext : DbContext
    {
        private string _databasePath;

        public DbSet<AgrYear> AgrYears { get; set; }
        public DbSet<Applicator> Applicators { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Field> Fields { get; set; }
        //public DbSet<Mixture> Mixtures { get; set; }
        public DbSet<Models.Object> Objects { get; set; }
        public DbSet<ProcessingType> ProcessingTypes { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeComponent> RecipeComponents { get; set; }
        public DbSet<Transport> Transports { get; set; }

        public AppDbContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}
