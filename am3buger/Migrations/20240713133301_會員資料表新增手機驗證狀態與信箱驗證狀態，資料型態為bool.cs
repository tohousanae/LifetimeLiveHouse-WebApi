using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 會員資料表新增手機驗證狀態與信箱驗證狀態資料型態為bool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailValidation",
                table: "User",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneValidation",
                table: "User",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailValidation",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneValidation",
                table: "User");
        }
    }
}
