using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcommerceLifestyle.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "LastName", "PasswordHash", "Role" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "afzal@example.com", "Afzal", "Haroon", "$2a$11$Mt5PNdur41SlBIGBosZy0u1DeJ6z3avZ9ReGci5rG1.6mVUkg538C", "User" },
                    { 2, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "abhishek@example.com", "Abhishek", "Kumar", "$2a$11$Mt5PNdur41SlBIGBosZy0ukcVxRInybBi/HKtPOqJpA8z1xgnCtAy", "User" },
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sudhir@example.com", "Sudhir", "Reddy", "$2a$11$Mt5PNdur41SlBIGBosZy0uKD2/EzPqUCAE8t6l/ho8T027wok0wD.", "User" },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "demo@example.com", "Demo", "User", "$2a$11$Mt5PNdur41SlBIGBosZy0uyGr/eADcK7Y4Y4iTfDxyXgVvvPRVJw6", "User" },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "vendor@example.com", "Vendor", "Account", "$2a$11$Mt5PNdur41SlBIGBosZy0u.D3Al5BEyQsTng4LvhAOtvMs3DPTu62", "Vendor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
