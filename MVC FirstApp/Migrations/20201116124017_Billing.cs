using Microsoft.EntityFrameworkCore.Migrations;

namespace MVC_FirstApp.Migrations
{
    public partial class Billing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Billings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HourlyPay = table.Column<double>(type: "float", nullable: false),
                    HoursWorked = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillingId",
                table: "AspNetUsers",
                column: "BillingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Billings_BillingId",
                table: "AspNetUsers",
                column: "BillingId",
                principalTable: "Billings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Billings_BillingId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BillingId",
                table: "AspNetUsers");
        }
    }
}
