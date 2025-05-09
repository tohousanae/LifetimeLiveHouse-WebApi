using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class uder表新增MikuPoint欄位 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Delivery_fee",
                table: "OrderForm");

            migrationBuilder.DropColumn(
                name: "Service_charge",
                table: "OrderForm");

            migrationBuilder.AddColumn<int>(
                name: "MikuPoint",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_User_Name",
                table: "User",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MikuPoint",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "Delivery_fee",
                table: "OrderForm",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Service_charge",
                table: "OrderForm",
                type: "int",
                nullable: true);
        }
    }
}
