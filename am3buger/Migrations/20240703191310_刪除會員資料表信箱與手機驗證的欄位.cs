using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 刪除會員資料表信箱與手機驗證的欄位 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVaildation",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneVaildation",
                table: "User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailVaildation",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneVaildation",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
