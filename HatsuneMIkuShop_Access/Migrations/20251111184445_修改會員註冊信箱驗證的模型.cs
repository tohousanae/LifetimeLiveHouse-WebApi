using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LifetimeLiveHouse.Access.Migrations
{
    /// <inheritdoc />
    public partial class 修改會員註冊信箱驗證的模型 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmailVerificationTokenHash",
                table: "MemberEmailVerificationStatus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmailVerificationTokenHash",
                table: "MemberEmailVerificationStatus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
