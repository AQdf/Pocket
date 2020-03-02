using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Migrations
{
    public partial class AddAssetValueAndUpdatedOnColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Asset",
                type: "datetime2(7)",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "Asset",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Asset");
        }
    }
}
