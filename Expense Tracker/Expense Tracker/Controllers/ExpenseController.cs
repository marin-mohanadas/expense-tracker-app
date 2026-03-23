using Expense_Tracker.Data;
using Expense_Tracker.DTOs.Expense;
using Expense_Tracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expense_Tracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpenseController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExpenseController(AppDbContext context)
    {
        _context = context;
    }

    // Helper method
    private User GetCurrentUser()
    {
        var username = User.Identity?.Name;
        return _context.Users.FirstOrDefault(u => u.Username == username);
    }

    // GET with filters
    [HttpGet]
    public IActionResult GetExpenses(
        [FromQuery] string? filter,
        [FromQuery] DateTime? start,
        [FromQuery] DateTime? end)
    {
        var user = GetCurrentUser();
        if (user == null)
            return Unauthorized();

        var query = _context.Expenses
            .AsNoTracking()
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

        var result = query.Select(e => new ExpenseResponse
        {
            ExpenseId = e.ExpenseId,
            ExpenseName = e.ExpenseName,
            Amount = e.Amount,
            UpdatedAt = e.UpdatedAt
        }).ToList();

        return Ok(result);
    }

    // POST
    [HttpPost]
    public IActionResult AddExpense([FromBody] ExpenseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = GetCurrentUser();
        if (user == null)
            return Unauthorized();

        var username = User.Identity?.Name;

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

        var response = new ExpenseResponse
        {
            ExpenseId = expense.ExpenseId,
            ExpenseName = expense.ExpenseName,
            Amount = expense.Amount,
            UpdatedAt = expense.UpdatedAt
        };

        return Ok(response);
    }

    // PUT
    [HttpPut("{id:int}")]
    public IActionResult UpdateExpense(int id, [FromBody] ExpenseRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = GetCurrentUser();
        if (user == null)
            return Unauthorized();

        var username = User.Identity?.Name;

        var expense = _context.Expenses
            .FirstOrDefault(e => e.ExpenseId == id && e.UserId == user.UserId);

        if (expense == null)
            return NotFound();

        expense.ExpenseName = request.ExpenseName;
        expense.Amount = request.Amount;
        expense.UpdatedAt = DateTime.UtcNow;
        expense.UpdatedBy = username;

        _context.SaveChanges();

        var response = new ExpenseResponse
        {
            ExpenseId = expense.ExpenseId,
            ExpenseName = expense.ExpenseName,
            Amount = expense.Amount,
            UpdatedAt = expense.UpdatedAt
        };

        return Ok(response);
    }

    // DELETE
    [HttpDelete("{id:int}")]
    public IActionResult DeleteExpense(int id)
    {
        var user = GetCurrentUser();
        if (user == null)
            return Unauthorized();

        var expense = _context.Expenses
            .FirstOrDefault(e => e.ExpenseId == id && e.UserId == user.UserId);

        if (expense == null)
            return NotFound();

        _context.Expenses.Remove(expense);
        _context.SaveChanges();

        return Ok(new { message = "Deleted successfully" });
    }
}