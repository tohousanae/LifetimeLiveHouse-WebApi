using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace am3burger.Migrations
{
    /// <inheritdoc />
    public partial class 新增商品名稱商品描述的索引 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Product_Name_Description",
                table: "Product",
                columns: new[] { "Name", "Description" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Product_Name_Description",
                table: "Product");
        }
    }
}
