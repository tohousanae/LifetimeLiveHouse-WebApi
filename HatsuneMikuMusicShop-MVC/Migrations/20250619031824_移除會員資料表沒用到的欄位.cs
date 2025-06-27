using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HatsuneMikuMusicShop_MVC.Migrations
{
    /// <inheritdoc />
    public partial class 移除會員資料表沒用到的欄位 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailValidation",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneValidation",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "MikuPoint",
                table: "User",
                newName: "MikuMikuPoint");

            migrationBuilder.AlterColumn<bool>(
                name: "Sex",
                table: "User",
                type: "bit",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MikuMikuPoint",
                table: "User",
                newName: "MikuPoint");

            migrationBuilder.AlterColumn<string>(
                name: "Sex",
                table: "User",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<bool>(
                name: "EmailValidation",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Identity",
                table: "User",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "PhoneValidation",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
