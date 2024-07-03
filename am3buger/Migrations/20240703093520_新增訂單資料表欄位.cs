using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 新增訂單資料表欄位 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "OrderForm",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Delivery_fee",
                table: "OrderForm",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderForm",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "OrderForm",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "OrderForm",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Service_charge",
                table: "OrderForm",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Store",
                table: "OrderForm",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "payment",
                table: "OrderForm",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "total_price",
                table: "OrderForm",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Delivery_fee",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Service_charge",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Store",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "payment",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "total_price",
                table: "OrderForm");
        }
    }
}
