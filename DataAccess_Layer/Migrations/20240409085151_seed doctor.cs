using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seeddoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.InsertData(
                table: "DoctorSpecialization",
                columns: new[] { "Id", "Specialization", "UserId" },
                values: new object[] { 1, "BrainSurgery", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DoctorSpecialization",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Address", "DateOfBirth", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "PostalCode", "Role", "Sex" },
                values: new object[,]
                {
                    { 2, "123 Main St", new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "parangi.rathod@bacancy.com", "Parangi", "Rathod", "password", "123-456-7890", "12345", "Doctor", "Female" },
                    { 3, "123 Main St", new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "khushbu.oza@bacancy.com", "Khushbu", "Oza", "password", "123-456-7890", "12345", "Doctor", "Female" },
                    { 4, "123 Main St", new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kishan.prajapati@bacancy.com", "Kishan", "Prajapati", "password", "123-456-7890", "12345", "Patient", "Male" }
                });
        }
    }
}
