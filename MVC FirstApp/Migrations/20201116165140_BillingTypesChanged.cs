using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_FirstApp.Migrations
{
    public partial class BillingTypesChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoursWorked",
                table: "Billings");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Billings",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,12)");

            migrationBuilder.AddColumn<long>(
                name: "MinutesWorked",
                table: "Billings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinutesWorked",
                table: "Billings");

            migrationBuilder.AlterColumn<decimal>(
                name: "Balance",
                table: "Billings",
                type: "decimal(18,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "HoursWorked",
                table: "Billings",
                type: "decimal(18,12)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
