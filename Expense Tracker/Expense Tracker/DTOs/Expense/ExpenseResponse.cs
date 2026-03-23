namespace Expense_Tracker.DTOs.Expense;

public class ExpenseResponse
{
    public int ExpenseId { get; set; }
    public string ExpenseName { get; set; }
    public double Amount { get; set; }
    public DateTime CreatedAt { get; set; }
}