using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Infrastructure.Persistence.Paycycles.Migrations
{
    public partial class Setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayCode",
                table: "PayInstruction");

            migrationBuilder.AddColumn<string>(
                name: "PayCodeCode",
                table: "PayInstruction",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PayCodeCustomerId",
                table: "PayInstruction",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayInstruction_PayCodeCustomerId_PayCodeCode",
                table: "PayInstruction",
                columns: new[] { "PayCodeCustomerId", "PayCodeCode" });

            migrationBuilder.AddForeignKey(
                name: "FK_PayInstruction_PayCodes_PayCodeCustomerId_PayCodeCode",
                table: "PayInstruction",
                columns: new[] { "PayCodeCustomerId", "PayCodeCode" },
                principalTable: "PayCodes",
                principalColumns: new[] { "CustomerId", "Code" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayInstruction_PayCodes_PayCodeCustomerId_PayCodeCode",
                table: "PayInstruction");

            migrationBuilder.DropIndex(
                name: "IX_PayInstruction_PayCodeCustomerId_PayCodeCode",
                table: "PayInstruction");

            migrationBuilder.DropColumn(
                name: "PayCodeCode",
                table: "PayInstruction");

            migrationBuilder.DropColumn(
                name: "PayCodeCustomerId",
                table: "PayInstruction");

            migrationBuilder.AddColumn<string>(
                name: "PayCode",
                table: "PayInstruction",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
