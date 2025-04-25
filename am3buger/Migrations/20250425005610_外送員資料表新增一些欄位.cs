using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 外送員資料表新增一些欄位 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Store",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DeliveryBoy",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "store_id",
                table: "DeliveryBoy",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "store_id",
                table: "DeliveryBoy");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Store",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "DeliveryBoy",
                newName: "Id");
        }
    }
}
