using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_FirstApp.Migrations
{
    public partial class BillingDecimalUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "HoursWorked",
                table: "Billings",
                type: "decimal(18,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Billings",
                type: "decimal(18,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "HoursWorked",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,12)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,12)");
        }
    }
}
