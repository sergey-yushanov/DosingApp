using DosingApp.Models;
using DosingApp.Models.AgroAssist.Data;
using Microsoft.EntityFrameworkCore;

namespace DosingApp.DataContext
{
    public class AppDbContext : DbContext
    {
        readonly private string _dbPath;

        public DbSet<Mixer> Mixers { get; set; }
        public DbSet<AgrYear> AgrYears { get; set; }
        public DbSet<Applicator> Applicators { get; set; }
        public DbSet<ApplicatorTank> ApplicatorTanks { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobComponent> JobComponents { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityTank> FacilityTanks { get; set; }
        public DbSet<ProcessingType> ProcessingTypes { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeComponent> RecipeComponents { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<TransportTank> TransportTanks { get; set; }

        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportComponent> ReportComponents { get; set; }

        public DbSet<Unit> Units { get; set; }
        public DbSet<Pesticide> Pesticides { get; set; }
        public DbSet<TechOperation> TechOperations { get; set; }
        public DbSet<PlanInventory> PlanInventories { get; set; }
        public DbSet<Withdrawal> Withdrawals { get; set; }

        public AppDbContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        public AppDbContext()
        {
            _dbPath = "";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_dbPath}");
        }
    }
}
