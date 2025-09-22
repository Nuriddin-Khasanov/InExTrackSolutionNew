using InExTrack.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InExTrack.Infrastructure.DataContext
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options): base(options) 
        { 
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserCategory> UserCategories { get; set; }
        public DbSet<Transaction_> Transactions { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<CategoryFile> CategotyFiles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration can go here
        }
    }
}
