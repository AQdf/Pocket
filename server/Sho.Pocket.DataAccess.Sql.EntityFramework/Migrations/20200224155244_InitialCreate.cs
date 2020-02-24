using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sho.Pocket.DataAccess.Sql.EntityFramework.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BalanceNote",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceNote", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Name = table.Column<string>(type: "char(3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Currency = table.Column<string>(type: "char(3)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asset_Currency_Currency",
                        column: x => x.Currency,
                        principalTable: "Currency",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExchangeRate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    BaseCurrency = table.Column<string>(type: "char(3)", nullable: false),
                    CounterCurrency = table.Column<string>(type: "char(3)", nullable: false),
                    BuyRate = table.Column<decimal>(type: "money", nullable: false),
                    SellRate = table.Column<decimal>(type: "money", nullable: false),
                    Provider = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeRate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeRate_Currency_BaseCurrency",
                        column: x => x.BaseCurrency,
                        principalTable: "Currency",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeRate_Currency_CounterCurrency",
                        column: x => x.CounterCurrency,
                        principalTable: "Currency",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCurrency",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<string>(type: "char(3)", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCurrency", x => new { x.UserId, x.Currency });
                    table.ForeignKey(
                        name: "FK_UserCurrency_Currency_Currency",
                        column: x => x.Currency,
                        principalTable: "Currency",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetBankAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AssetId = table.Column<Guid>(nullable: false),
                    BankName = table.Column<string>(type: "varchar(50)", nullable: false),
                    BankAccountId = table.Column<string>(type: "varchar(200)", nullable: true),
                    LastSyncDateTime = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    BankAccountName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankClientId = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetBankAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetBankAccount_Asset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AssetId = table.Column<Guid>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    Value = table.Column<decimal>(type: "money", nullable: false),
                    ExchangeRateId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balance_Asset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "Asset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Balance_ExchangeRate_ExchangeRateId",
                        column: x => x.ExchangeRateId,
                        principalTable: "ExchangeRate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asset_Currency",
                table: "Asset",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_AssetBankAccount_AssetId",
                table: "AssetBankAccount",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_AssetId",
                table: "Balance",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Balance_ExchangeRateId",
                table: "Balance",
                column: "ExchangeRateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRate_BaseCurrency",
                table: "ExchangeRate",
                column: "BaseCurrency");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRate_CounterCurrency",
                table: "ExchangeRate",
                column: "CounterCurrency");

            migrationBuilder.CreateIndex(
                name: "IX_UserCurrency_Currency",
                table: "UserCurrency",
                column: "Currency");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssetBankAccount");

            migrationBuilder.DropTable(
                name: "Balance");

            migrationBuilder.DropTable(
                name: "BalanceNote");

            migrationBuilder.DropTable(
                name: "UserCurrency");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "ExchangeRate");

            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
