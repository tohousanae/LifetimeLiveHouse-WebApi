using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 修改訂單資料表 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoName",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Store",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Total_price",
                table: "OrderForm");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoName",
                table: "OrderForm",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "OrderForm",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "OrderForm",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Store",
                table: "OrderForm",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Total_price",
                table: "OrderForm",
                type: "int",
                nullable: true);
        }
    }
}
