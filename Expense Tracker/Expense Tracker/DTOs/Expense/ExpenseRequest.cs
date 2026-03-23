using System.ComponentModel.DataAnnotations;

namespace Expense_Tracker.DTOs.Expense;

public class ExpenseRequest
{
    [Required]
    public string Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public double Amount { get; set; }

    [Required]
    public int CategoryId { get; set; }
}