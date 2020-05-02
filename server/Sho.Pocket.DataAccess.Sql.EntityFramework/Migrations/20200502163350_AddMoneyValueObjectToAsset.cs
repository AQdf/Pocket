using Microsoft.EntityFrameworkCore.Migrations;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Migrations
{
    public partial class AddMoneyValueObjectToAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Asset",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Asset",
                type: "char(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(3)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Asset",
                type: "money",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "money",
                oldNullable: false,
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Currency",
                table: "Asset",
                type: "char(3)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(3)",
                oldNullable: false);
        }
    }
}
