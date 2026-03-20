using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("UserInfo");

            entity.HasIndex(u => u.Username)
                .IsUnique();
            
            entity.Property(u => u.Username)
                .HasMaxLength(100);

            entity.Property(u => u.PasswordHash)
                .HasMaxLength(255);
        });
    }
}