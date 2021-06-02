using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Infrastructure.Persistence.Paycycles.Migrations
{
    public partial class FixPayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payees_Paycycles_PaycycleId",
                table: "Payees");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentOptions_Payees_PayeeId",
                table: "PaymentOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentOptions",
                table: "PaymentOptions");

            migrationBuilder.DropIndex(
                name: "IX_PaymentOptions_PayeeId",
                table: "PaymentOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payees",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "PaymentOptionsId",
                table: "PaymentOptions");

            migrationBuilder.DropColumn(
                name: "PayeeId",
                table: "PaymentOptions");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Paycycles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Paycycles");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Paycycles");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Paycycles");

            migrationBuilder.DropColumn(
                name: "PayeeId",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Payees");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Payees");

            migrationBuilder.RenameTable(
                name: "Payees",
                newName: "Payee");

            migrationBuilder.RenameIndex(
                name: "IX_Payees_PaycycleId",
                table: "Payee",
                newName: "IX_Payee_PaycycleId");

            migrationBuilder.AlterColumn<string>(
                name: "IsoCountryCode",
                table: "PaymentOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "PaymentOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountHolder",
                table: "PaymentOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNumber",
                table: "PaymentOptions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Paycycles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeNumber",
                table: "Payee",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentOptions",
                table: "PaymentOptions",
                column: "EmployeeNumber");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payee",
                table: "Payee",
                column: "EmployeeNumber");

            migrationBuilder.CreateTable(
                name: "PayInstruction",
                columns: table => new
                {
                    InstructionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitAmount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnitAmount_Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount_Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayeeEmployeeNumber = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayInstruction", x => x.InstructionId);
                    table.ForeignKey(
                        name: "FK_PayInstruction_Payee_PayeeEmployeeNumber",
                        column: x => x.PayeeEmployeeNumber,
                        principalTable: "Payee",
                        principalColumn: "EmployeeNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayInstruction_PayeeEmployeeNumber",
                table: "PayInstruction",
                column: "PayeeEmployeeNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Payee_Paycycles_PaycycleId",
                table: "Payee",
                column: "PaycycleId",
                principalTable: "Paycycles",
                principalColumn: "PaycycleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentOptions_Payee_EmployeeNumber",
                table: "PaymentOptions",
                column: "EmployeeNumber",
                principalTable: "Payee",
                principalColumn: "EmployeeNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payee_Paycycles_PaycycleId",
                table: "Payee");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentOptions_Payee_EmployeeNumber",
                table: "PaymentOptions");

            migrationBuilder.DropTable(
                name: "PayInstruction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentOptions",
                table: "PaymentOptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payee",
                table: "Payee");

            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "PaymentOptions");

            migrationBuilder.DropColumn(
                name: "EmployeeNumber",
                table: "Payee");

            migrationBuilder.RenameTable(
                name: "Payee",
                newName: "Payees");

            migrationBuilder.RenameIndex(
                name: "IX_Payee_PaycycleId",
                table: "Payees",
                newName: "IX_Payees_PaycycleId");

            migrationBuilder.AlterColumn<string>(
                name: "IsoCountryCode",
                table: "PaymentOptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "PaymentOptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountHolder",
                table: "PaymentOptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentOptionsId",
                table: "PaymentOptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayeeId",
                table: "PaymentOptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Paycycles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Paycycles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Paycycles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Paycycles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Paycycles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PayeeId",
                table: "Payees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Payees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Payees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "Payees",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Payees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Payees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentOptions",
                table: "PaymentOptions",
                column: "PaymentOptionsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payees",
                table: "Payees",
                column: "PayeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOptions_PayeeId",
                table: "PaymentOptions",
                column: "PayeeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payees_Paycycles_PaycycleId",
                table: "Payees",
                column: "PaycycleId",
                principalTable: "Paycycles",
                principalColumn: "PaycycleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentOptions_Payees_PayeeId",
                table: "PaymentOptions",
                column: "PayeeId",
                principalTable: "Payees",
                principalColumn: "PayeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
