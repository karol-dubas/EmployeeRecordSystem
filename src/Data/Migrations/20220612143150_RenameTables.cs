using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeRecordSystem.Data.Migrations
{
    public partial class RenameTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "requests");

            migrationBuilder.DropTable(
                name: "user-operations");

            migrationBuilder.DeleteData(
                table: "request-status-types",
                keyColumn: "Code",
                keyValue: "new");

            migrationBuilder.CreateTable(
                name: "balance-logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_balance-logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_balance-logs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "withdrawal-requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WithdrawalRequestStatusTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_withdrawal-requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_withdrawal-requests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_withdrawal-requests_request-status-types_WithdrawalRequestStatusTypeCode",
                        column: x => x.WithdrawalRequestStatusTypeCode,
                        principalTable: "request-status-types",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "request-status-types",
                columns: new[] { "Code", "Name" },
                values: new object[] { "pending", "Pending" });

            migrationBuilder.CreateIndex(
                name: "IX_balance-logs_UserId",
                table: "balance-logs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_withdrawal-requests_UserId",
                table: "withdrawal-requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_withdrawal-requests_WithdrawalRequestStatusTypeCode",
                table: "withdrawal-requests",
                column: "WithdrawalRequestStatusTypeCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "balance-logs");

            migrationBuilder.DropTable(
                name: "withdrawal-requests");

            migrationBuilder.DeleteData(
                table: "request-status-types",
                keyColumn: "Code",
                keyValue: "pending");

            migrationBuilder.CreateTable(
                name: "requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestStatusTypeCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ProcessedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_requests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_requests_request-status-types_RequestStatusTypeCode",
                        column: x => x.RequestStatusTypeCode,
                        principalTable: "request-status-types",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user-operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user-operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user-operations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "request-status-types",
                columns: new[] { "Code", "Name" },
                values: new object[] { "new", "New" });

            migrationBuilder.CreateIndex(
                name: "IX_requests_RequestStatusTypeCode",
                table: "requests",
                column: "RequestStatusTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_requests_UserId",
                table: "requests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user-operations_UserId",
                table: "user-operations",
                column: "UserId");
        }
    }
}
