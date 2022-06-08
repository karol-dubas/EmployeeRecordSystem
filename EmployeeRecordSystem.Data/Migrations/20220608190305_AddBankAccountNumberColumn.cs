using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeRecordSystem.Data.Migrations
{
    public partial class AddBankAccountNumberColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountNumber",
                table: "AspNetUsers",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountNumber",
                table: "AspNetUsers");
        }
    }
}
