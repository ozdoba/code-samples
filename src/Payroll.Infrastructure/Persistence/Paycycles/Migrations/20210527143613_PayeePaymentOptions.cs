using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payroll.Infrastructure.Persistence.Paycycles.Migrations
{
    public partial class PayeePaymentOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payees",
                columns: table => new
                {
                    PayeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaycycleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payees", x => x.PayeeId);
                    table.ForeignKey(
                        name: "FK_Payees_Paycycles_PaycycleId",
                        column: x => x.PaycycleId,
                        principalTable: "Paycycles",
                        principalColumn: "PaycycleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentOptions",
                columns: table => new
                {
                    PaymentOptionsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountHolder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SwiftCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchAddress_BuildingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchAddress_PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchAddress_CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsoCountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOptions", x => x.PaymentOptionsId);
                    table.ForeignKey(
                        name: "FK_PaymentOptions_Payees_PayeeId",
                        column: x => x.PayeeId,
                        principalTable: "Payees",
                        principalColumn: "PayeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payees_PaycycleId",
                table: "Payees",
                column: "PaycycleId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentOptions_PayeeId",
                table: "PaymentOptions",
                column: "PayeeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentOptions");

            migrationBuilder.DropTable(
                name: "Payees");
        }
    }
}
