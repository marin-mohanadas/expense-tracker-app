using Microsoft.EntityFrameworkCore;
using Expense_Tracker.Models;

namespace Expense_Tracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }

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

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.ToTable("ExpenseInfo");

            entity.Property(e => e.ExpenseName)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.Amount)
                .IsRequired();

            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId);
        });
    }
}