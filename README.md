# Expense Tracker API

A simple Expense Tracker API built with ASP.NET Core, Entity Framework Core, and JWT authentication.

---

## Features

- User management
  - Register / login with JWT
  - Passwords hashed using BCrypt
- Expense managementß
  - CRUD operations for expenses
  - Each expense has a description, amount, category, and timestamps
  - User-specific data isolation
  - Filter expenses by date ranges (week, month, 3 months, custom)
  - Filter by category
- Categories
  - Predefined categories: Groceries, Leisure, Electronics, Utilities, Clothing, Health, Others
- JWT Authentication
  - Secure endpoints with `[Authorize]`
  - JWT Bearer support in Swagger UI
- Database
  - SQL Server with EF Core (Code-First)
  - Auto migrations and seeded categories
- Clean API design with DTOs

---

## Technologies

- .NET 9 / ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger / OpenAPI
- BCrypt password hashing

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server
- IDE: Visual Studio / Rider / VS Code

### Setup

1. Clone the repo:

```bash
git clone https://github.com/yourusername/expense-tracker-api.git
cd expense-tracker-api
```

2. Add your secrets in `appsettings.Secrets.json` (never commit secrets):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ExpenseTracker;User Id=USER;Password=PASS;"
  },
  "Jwt": {
    "Key": "your_super_secret_key_with_at_least_32_chars",
    "Issuer": "ExpenseTrackerAPI",
    "Audience": "ExpenseTrackerAPIUsers"
  }
}
```

3. Apply migrations:

```bash
dotnet ef database update
```

4. Run the API:

```bash
dotnet run
```

5. Open Swagger UI (development mode):

```https
https://localhost:7050/swagger
```

### Usage

- Register a new user via `/api/auth/register`
- Login via `/api/auth/login` to get JWT token
- Add JWT in Swagger `Authorize` button or use Bearer token in Postman
- Manage expenses via `/api/expense`


### Folder Structure

```
Expense-Tracker-API/
│
├─ Controllers/        # API controllers
├─ Data/               # DbContext and EF configurations
├─ DTOs/               # Request and Response DTOs
├─ Models/             # Database models
├─ Services/           # Authentication services
├─ Migrations/         # EF migrations
├─ Program.cs          # App startup
├─ appsettings.json
├─ appsettings.Secrets.json
└─ README.md
```

---

## Notes

- Never commit `appsettings.Secrets.json` or hardcoded secrets.
- All migrations are code-first; SQL changes are handled by EF.
- Each user can only see their own expenses.

---

## Future Improvements

- Refresh tokens
- Pagination for expense listing
- Search by description
- Expense summary endpoints (total spending, spending per category)
- Unit & integration tests

---

## 📚 Requirements (roadmap.sh)

This project follows the official requirements from:
https://roadmap.sh/projects/expense-tracker-api

---

## 👨‍💻 Author

- GitHub: https://github.com/marin-mohanadas

---

## 📄 License

This project is open source under the MIT License.
