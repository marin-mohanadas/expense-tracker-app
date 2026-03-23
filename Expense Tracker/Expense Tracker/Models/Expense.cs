namespace Expense_Tracker.Models;

public class Expense
{
    public int ExpenseId { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public ExpenseCategory Category { get; set; }
    public double Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string UpdatedBy { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}