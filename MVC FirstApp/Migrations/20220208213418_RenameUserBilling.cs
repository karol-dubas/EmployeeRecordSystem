using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_FirstApp.Migrations
{
    public partial class RenameUserBilling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Billings_BillingId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Billings");

            migrationBuilder.DropIndex(
                name: "IX_Users_BillingId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "BillingId",
                table: "Users",
                newName: "UserBillingId");

            migrationBuilder.CreateTable(
                name: "UserBillings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HourlyPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinutesWorked = table.Column<long>(type: "bigint", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBillings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserBillingId",
                table: "Users",
                column: "UserBillingId",
                unique: true,
                filter: "[UserBillingId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserBillings_UserBillingId",
                table: "Users",
                column: "UserBillingId",
                principalTable: "UserBillings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserBillings_UserBillingId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UserBillings");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserBillingId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserBillingId",
                table: "Users",
                newName: "BillingId");

            migrationBuilder.CreateTable(
                name: "Billings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HourlyPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinutesWorked = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Billings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_BillingId",
                table: "Users",
                column: "BillingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Billings_BillingId",
                table: "Users",
                column: "BillingId",
                principalTable: "Billings",
                principalColumn: "Id");
        }
    }
}
