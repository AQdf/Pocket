using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Migrations
{
    public partial class RemoveBalanceExchangeRateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balance_ExchangeRate_ExchangeRateId",
                table: "Balance");

            migrationBuilder.DropIndex(
                name: "IX_Balance_ExchangeRateId",
                table: "Balance");

            migrationBuilder.DropColumn(
                name: "ExchangeRateId",
                table: "Balance");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExchangeRateId",
                table: "Balance",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Balance_ExchangeRateId",
                table: "Balance",
                column: "ExchangeRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Balance_ExchangeRate_ExchangeRateId",
                table: "Balance",
                column: "ExchangeRateId",
                principalTable: "ExchangeRate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
