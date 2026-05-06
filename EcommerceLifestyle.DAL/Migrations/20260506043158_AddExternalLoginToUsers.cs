using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceLifestyle.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalLoginToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "Users",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Local")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProviderUserId",
                table: "Users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Provider", "ProviderUserId" },
                values: new object[] { "Local", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Provider", "ProviderUserId" },
                values: new object[] { "Local", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Provider", "ProviderUserId" },
                values: new object[] { "Local", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Provider", "ProviderUserId" },
                values: new object[] { "Local", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Provider", "ProviderUserId" },
                values: new object[] { "Local", null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Provider_ProviderUserId",
                table: "Users",
                columns: new[] { "Provider", "ProviderUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Provider_ProviderUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProviderUserId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "PasswordHash",
                keyValue: null,
                column: "PasswordHash",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
