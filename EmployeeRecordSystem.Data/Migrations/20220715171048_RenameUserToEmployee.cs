using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeRecordSystem.Data.Migrations
{
    public partial class RenameUserToEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_balance-logs_AspNetUsers_UserId",
                table: "balance-logs");

            migrationBuilder.DropForeignKey(
                name: "FK_withdrawal-requests_AspNetUsers_UserId",
                table: "withdrawal-requests");

            migrationBuilder.DropTable(
                name: "user-billings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "withdrawal-requests",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_withdrawal-requests_UserId",
                table: "withdrawal-requests",
                newName: "IX_withdrawal-requests_EmployeeId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "balance-logs",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_balance-logs_UserId",
                table: "balance-logs",
                newName: "IX_balance-logs_EmployeeId");

            migrationBuilder.CreateTable(
                name: "employee-billings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HourlyPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TimeWorked = table.Column<long>(type: "bigint", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employee-billings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_employee-billings_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employee-billings_EmployeeId",
                table: "employee-billings",
                column: "EmployeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_balance-logs_AspNetUsers_EmployeeId",
                table: "balance-logs",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_withdrawal-requests_AspNetUsers_EmployeeId",
                table: "withdrawal-requests",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_balance-logs_AspNetUsers_EmployeeId",
                table: "balance-logs");

            migrationBuilder.DropForeignKey(
                name: "FK_withdrawal-requests_AspNetUsers_EmployeeId",
                table: "withdrawal-requests");

            migrationBuilder.DropTable(
                name: "employee-billings");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "withdrawal-requests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_withdrawal-requests_EmployeeId",
                table: "withdrawal-requests",
                newName: "IX_withdrawal-requests_UserId");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "balance-logs",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_balance-logs_EmployeeId",
                table: "balance-logs",
                newName: "IX_balance-logs_UserId");

            migrationBuilder.CreateTable(
                name: "user-billings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    HourlyPay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TimeWorked = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user-billings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user-billings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user-billings_UserId",
                table: "user-billings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_balance-logs_AspNetUsers_UserId",
                table: "balance-logs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_withdrawal-requests_AspNetUsers_UserId",
                table: "withdrawal-requests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
