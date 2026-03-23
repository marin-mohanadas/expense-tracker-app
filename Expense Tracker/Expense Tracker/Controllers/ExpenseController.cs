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

    // Helper: Get logged-in user
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
        [FromQuery] DateTime? end,
        [FromQuery] int? categoryId)
    {
        var user = GetCurrentUser();
        if (user == null)
            return Unauthorized();

        var query = _context.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .Where(e => e.UserId == user.UserId);

        var now = DateTime.UtcNow;

        // Time filters
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

        // Category filter
        if (categoryId.HasValue)
        {
            query = query.Where(e => e.CategoryId == categoryId.Value);
        }

        var result = query.Select(e => new ExpenseResponse
        {
            ExpenseId = e.ExpenseId,
            Description = e.Description,
            Amount = e.Amount,
            CategoryName = e.Category.CategoryName,
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

        var category = _context.Set<ExpenseCategory>()
            .FirstOrDefault(c => c.CategoryId == request.CategoryId);

        if (category == null)
            return BadRequest("Invalid category");

        var username = User.Identity?.Name;

        var expense = new Expense
        {
            Description = request.Description,
            Amount = request.Amount,
            CategoryId = request.CategoryId,
            UserId = user.UserId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = username
        };

        _context.Expenses.Add(expense);
        _context.SaveChanges();

        return Ok(new ExpenseResponse
        {
            ExpenseId = expense.ExpenseId,
            Description = expense.Description,
            Amount = expense.Amount,
            CategoryName = category.CategoryName,
            UpdatedAt = expense.UpdatedAt
        });
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

        var expense = _context.Expenses
            .FirstOrDefault(e => e.ExpenseId == id && e.UserId == user.UserId);

        if (expense == null)
            return NotFound();

        var category = _context.Set<ExpenseCategory>()
            .FirstOrDefault(c => c.CategoryId == request.CategoryId);

        if (category == null)
            return BadRequest("Invalid category");

        var username = User.Identity?.Name;

        expense.Description = request.Description;
        expense.Amount = request.Amount;
        expense.CategoryId = request.CategoryId;
        expense.UpdatedAt = DateTime.UtcNow;
        expense.UpdatedBy = username;

        _context.SaveChanges();

        return Ok(new ExpenseResponse
        {
            ExpenseId = expense.ExpenseId,
            Description = expense.Description,
            Amount = expense.Amount,
            CategoryName = category.CategoryName,
            UpdatedAt = expense.UpdatedAt
        });
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

    // GET Categories (helper endpoint)
    [HttpGet("categories")]
    public IActionResult GetCategories()
    {
        var categories = _context.Set<ExpenseCategory>()
            .Select(c => new { c.CategoryId, c.CategoryName })
            .ToList();

        return Ok(categories);
    }
}