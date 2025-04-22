using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 資料表已有主鍵作為作為叢集索引鍵不需要另外設置索引因此將資料庫除主鍵外的索引刪除 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IX_User_Name",
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

            migrationBuilder.DropIndex(
                name: "IX_Product_Name_Description_Type",
                table: "Product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_User_Name",
                table: "User",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

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

            migrationBuilder.CreateIndex(
                name: "IX_Product_Name_Description_Type",
                table: "Product",
                columns: new[] { "Name", "Description", "Type" });
        }
    }
}
