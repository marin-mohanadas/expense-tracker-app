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
    public DbSet<ExpenseCategory> ExpenseCategories { get; set; }

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

            entity.Property(e => e.Description)
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
            
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId);
        });

        modelBuilder.Entity<ExpenseCategory>(entity =>
        {
            entity.ToTable("ExpenseCategory");
            
            entity.HasKey(c => c.CategoryId);

            entity.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(c => c.CategoryName)
                .IsUnique(); // prevents duplicates
        });
        
        modelBuilder.Entity<ExpenseCategory>().HasData(
            new ExpenseCategory { CategoryId = 1, CategoryName = "Groceries" },
            new ExpenseCategory { CategoryId = 2, CategoryName = "Leisure" },
            new ExpenseCategory { CategoryId = 3, CategoryName = "Electronics" },
            new ExpenseCategory { CategoryId = 4, CategoryName = "Utilities" },
            new ExpenseCategory { CategoryId = 5, CategoryName = "Clothing" },
            new ExpenseCategory { CategoryId = 6, CategoryName = "Health" },
            new ExpenseCategory { CategoryId = 7, CategoryName = "Others" }
        );
    }
}