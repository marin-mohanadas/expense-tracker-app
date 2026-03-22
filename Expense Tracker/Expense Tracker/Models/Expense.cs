namespace Expense_Tracker.Models;

public class Expense
{
    public int ExpenseId { get; set; }
    public string ExpenseName { get; set; }
    public double Amount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string UpdatedBy { get; set; }
    
    // Link to User
    public int UserId { get; set; }
    public User User { get; set; }
}