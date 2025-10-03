using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataContext;

public sealed class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) 
    { 
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction_> Transactions { get; set; }
    public DbSet<UserFile> UserFiles { get; set; }
    public DbSet<CategoryFile> CategotyFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Additional configuration can go here

        modelBuilder.Entity<User>()
            .HasOne(u => u.Image)
            .WithOne(f => f.User)
            .HasForeignKey<UserFile>(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .Property(c => c.Type)
            .HasConversion<int>(); // хранит как int (1 или 2)
    }
}
