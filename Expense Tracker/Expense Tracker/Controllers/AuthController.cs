using Microsoft.AspNetCore.Mvc;
using Expense_Tracker.Data;
using Expense_Tracker.Models;
using Expense_Tracker.DTOs.Auth;
using Expense_Tracker.Services;

namespace Expense_Tracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly AuthService _authService;

    public AuthController(AppDbContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        var username = request.Username.ToLower();

        if (_context.Users.Any(u => u.Username == username))
        {
            return BadRequest("Username already exists");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Username = username,
            PasswordHash = hashedPassword
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("User registered");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var username = request.Username.ToLower();

        var user = _context.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
        {
            return Unauthorized("Invalid credentials");
        }

        var isValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isValid)
        {
            return Unauthorized("Invalid credentials");
        }

        if (!user.IsActive)
        {
            return Unauthorized("User is inactive");
        }

        var token = _authService.GenerateJwtToken(user.Username);

        return Ok(new { token });
    }

    
}