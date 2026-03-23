using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Expense_Tracker.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpenseName",
                table: "ExpenseInfo",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "ExpenseInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ExpenseCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategory", x => x.CategoryId);
                });

            migrationBuilder.InsertData(
                table: "ExpenseCategory",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Groceries" },
                    { 2, "Leisure" },
                    { 3, "Electronics" },
                    { 4, "Utilities" },
                    { 5, "Clothing" },
                    { 6, "Health" },
                    { 7, "Others" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseInfo_CategoryId",
                table: "ExpenseInfo",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategory_CategoryName",
                table: "ExpenseCategory",
                column: "CategoryName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseInfo_ExpenseCategory_CategoryId",
                table: "ExpenseInfo",
                column: "CategoryId",
                principalTable: "ExpenseCategory",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseInfo_ExpenseCategory_CategoryId",
                table: "ExpenseInfo");

            migrationBuilder.DropTable(
                name: "ExpenseCategory");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseInfo_CategoryId",
                table: "ExpenseInfo");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "ExpenseInfo");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ExpenseInfo",
                newName: "ExpenseName");
        }
    }
}
