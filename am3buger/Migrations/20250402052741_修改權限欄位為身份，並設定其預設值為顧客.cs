using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 修改權限欄位為身份並設定其預設值為顧客 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permission",
                table: "User");

            migrationBuilder.AddColumn<string>(
                name: "Identity",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "顧客");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identity",
                table: "User");

            migrationBuilder.AddColumn<int>(
                name: "Permission",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
