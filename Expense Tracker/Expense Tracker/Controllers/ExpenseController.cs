using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Expense_Tracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpenseController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public ExpenseController(AppDbContext context)
    {
        _context = context;
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult GetExpenses(string? filter, DateTime? start, DateTime? end)
    {
        var username = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        var query = _context.Expenses
            .Where(e => e.UserId == user.UserId);
        var now = DateTime.UtcNow;

        switch (filter?.ToLower())
        {
            case "week":
                query = query.Where(e => e.CreatedAt >= now.AddDays(-7));
                break;
            case "month":
                query = query.Where(e => e.CreatedAt >= now.AddMonths(-1));
                break;
            case "3months":
                query = query.Where(e => e.CreatedAt >= now.AddMonths(-3));
                break;
            case "custom":
                if (start.HasValue && end.HasValue)
                {
                    query = query.Where(e => e.CreatedAt >= start && e.CreatedAt <= end);
                }
                break;
        }

        return Ok(query.ToList());
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult AddExpense([FromBody] Expense request)
    {
        var username = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        var expense = new Expense
        {
            ExpenseName = request.ExpenseName,
            Amount = request.Amount,
            UserId = user.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = username
        };

        _context.Expenses.Add(expense);
        _context.SaveChanges();

        return Ok(expense);
    }
    
    [Authorize]
    [HttpPut("{id:int}")]
    public IActionResult UpdateExpense(int id, [FromBody] Expense request)
    {
        var username = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        var expense = _context.Expenses
            .FirstOrDefault(e => e.ExpenseId == id && e.UserId == user.UserId);
        
        if (expense == null)
            return NotFound();

        expense.ExpenseName = request.ExpenseName;
        expense.Amount = request.Amount;
        expense.UpdatedAt = DateTime.UtcNow; // ✅ IMPORTANT
        expense.UpdatedBy = username;

        _context.SaveChanges();

        return Ok(expense);
    }
    
    [Authorize]
    [HttpDelete("{id:int}")]
    public IActionResult DeleteExpense(int id)
    {
        var username = User.Identity?.Name;
        var user = _context.Users.FirstOrDefault(u => u.Username == username);
        var expense = _context.Expenses
            .FirstOrDefault(e => e.ExpenseId == id && e.UserId == user.UserId);
        
        if (expense == null)
            return NotFound();

        _context.Expenses.Remove(expense);
        _context.SaveChanges();

        return Ok("Deleted");
    }
    
}