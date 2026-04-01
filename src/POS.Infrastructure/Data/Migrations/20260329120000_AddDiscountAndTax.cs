using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using POS.Infrastructure.Data;

#nullable disable

namespace POS.Infrastructure.Data.Migrations;

[DbContext(typeof(PosDbContext))]
[Migration("20260329120000_AddDiscountAndTax")]
public sealed class AddDiscountAndTax : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Per-line discount percentage
        migrationBuilder.AddColumn<decimal>(
            name: "DiscountPercent",
            table: "InvoiceItems",
            type: "REAL",
            precision: 5,
            scale: 2,
            nullable: false,
            defaultValue: 0m);

        // Invoice-level tax percentage (e.g. VAT)
        migrationBuilder.AddColumn<decimal>(
            name: "TaxPercent",
            table: "Invoices",
            type: "REAL",
            precision: 5,
            scale: 2,
            nullable: false,
            defaultValue: 0m);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "DiscountPercent", table: "InvoiceItems");
        migrationBuilder.DropColumn(name: "TaxPercent",      table: "Invoices");
    }
}
