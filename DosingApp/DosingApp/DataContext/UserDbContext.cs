using DosingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DosingApp.DataContext
{
    public class UserDbContext : DbContext
    {
        private string _dbPath;

        public DbSet<User> Users { get; set; }

        public UserDbContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_dbPath}");
        }
    }
}
