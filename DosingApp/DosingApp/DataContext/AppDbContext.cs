﻿using DosingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DosingApp.DataContext
{
    public class AppDbContext : DbContext
    {
        private string _dbPath;

        public DbSet<AgrYear> AgrYears { get; set; }
        public DbSet<Applicator> Applicators { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Field> Fields { get; set; }
        //public DbSet<Mixture> Mixtures { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<ProcessingType> ProcessingTypes { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeComponent> RecipeComponents { get; set; }
        public DbSet<Transport> Transports { get; set; }

/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Crop>()
                .HasMany(c => c.Recipes)
                .WithOne(r => r.Crop)
                .OnDelete(DeleteBehavior.ClientSetNull);
                //.IsRequired(false);
        }*/



        public AppDbContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_dbPath}");
        }
    }
}
