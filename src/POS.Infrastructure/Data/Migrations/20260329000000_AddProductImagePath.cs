using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using POS.Infrastructure.Data;

#nullable disable

namespace POS.Infrastructure.Data.Migrations;

[DbContext(typeof(PosDbContext))]
[Migration("20260329000000_AddProductImagePath")]
public class AddProductImagePath : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "ImagePath",
            table: "Products",
            type: "TEXT",
            maxLength: 1000,
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "ImagePath", table: "Products");
    }
}
