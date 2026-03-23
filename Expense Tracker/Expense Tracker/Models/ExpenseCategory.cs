namespace Expense_Tracker.Models;

public class ExpenseCategory
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }

    public List<Expense> Expenses { get; set; }
}