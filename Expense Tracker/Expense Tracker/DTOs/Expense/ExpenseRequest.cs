using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.DTOs.Expense;

public class ExpenseRequest
{
    [Required]
    [MaxLength(200)]
    public string ExpenseName { get; set; }

    [Range(0.01, double.MaxValue)]
    public double Amount { get; set; }
}