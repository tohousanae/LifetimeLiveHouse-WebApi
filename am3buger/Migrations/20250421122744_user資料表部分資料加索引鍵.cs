using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class user資料表部分資料加索引鍵 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_User_Birthday",
                table: "User",
                column: "Birthday",
                unique: true,
                filter: "[Birthday] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_EmailValidation",
                table: "User",
                column: "EmailValidation",
                unique: true,
                filter: "[EmailValidation] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_Identity",
                table: "User",
                column: "Identity",
                unique: true,
                filter: "[Identity] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_PhoneNumber",
                table: "User",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_PhoneValidation",
                table: "User",
                column: "PhoneValidation",
                unique: true,
                filter: "[PhoneValidation] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_Sex",
                table: "User",
                column: "Sex",
                unique: true,
                filter: "[Sex] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_Birthday",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_EmailValidation",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Identity",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PhoneNumber",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PhoneValidation",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Sex",
                table: "User");
        }
    }
}
