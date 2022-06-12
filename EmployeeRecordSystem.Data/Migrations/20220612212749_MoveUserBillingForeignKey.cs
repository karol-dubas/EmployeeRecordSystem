using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeRecordSystem.Data.Migrations
{
    public partial class MoveUserBillingForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_user-billings_UserBillingId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserBillingId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserBillingId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "user-billings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_user-billings_UserId",
                table: "user-billings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user-billings_AspNetUsers_UserId",
                table: "user-billings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user-billings_AspNetUsers_UserId",
                table: "user-billings");

            migrationBuilder.DropIndex(
                name: "IX_user-billings_UserId",
                table: "user-billings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "user-billings");

            migrationBuilder.AddColumn<Guid>(
                name: "UserBillingId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserBillingId",
                table: "AspNetUsers",
                column: "UserBillingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_user-billings_UserBillingId",
                table: "AspNetUsers",
                column: "UserBillingId",
                principalTable: "user-billings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
