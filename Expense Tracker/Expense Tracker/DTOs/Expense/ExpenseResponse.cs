namespace Expense_Tracker.DTOs.Expense;

public class ExpenseResponse
{
    public int ExpenseId { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
    public string CategoryName { get; set; }
    public DateTime UpdatedAt { get; set; }
}