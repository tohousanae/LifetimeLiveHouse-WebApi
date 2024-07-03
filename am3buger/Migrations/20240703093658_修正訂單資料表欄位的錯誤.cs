using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 修正訂單資料表欄位的錯誤 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "total_price",
                table: "OrderForm",
                newName: "Total_price");

            migrationBuilder.RenameColumn(
                name: "payment",
                table: "OrderForm",
                newName: "Payment");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "OrderForm",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "OrderForm",
                newName: "OrderTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Total_price",
                table: "OrderForm",
                newName: "total_price");

            migrationBuilder.RenameColumn(
                name: "Payment",
                table: "OrderForm",
                newName: "payment");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "OrderForm",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "OrderTime",
                table: "OrderForm",
                newName: "Birthday");
        }
    }
}
