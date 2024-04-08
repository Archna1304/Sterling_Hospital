using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class datetimechanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ScheduleStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientProblem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultingDoctor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NurseId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_User_NurseId",
                        column: x => x.NurseId,
                        principalTable: "User",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_User_PatientId",
                        column: x => x.PatientId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DoctorSpecialization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSpecialization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorSpecialization_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Address", "DateOfBirth", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "PostalCode", "Role", "Sex" },
                values: new object[,]
                {
                    { 1, "123 Main St", new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "archana.vyas@bacancy.com", "Archana", "Vyas", "password", "123-456-7890", "12345", "Doctor", "Female" },
                    { 2, "123 Main St", new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "parangi.rathod@bacancy.com", "Parangi", "Rathod", "password", "123-456-7890", "12345", "Doctor", "Female" },
                    { 3, "123 Main St", new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "khushbu.oza@bacancy.com", "Khushbu", "Oza", "password", "123-456-7890", "12345", "Doctor", "Female" },
                    { 4, "123 Main St", new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "kishan.prajapati@bacancy.com", "Kishan", "Prajapati", "password", "123-456-7890", "12345", "Patient", "Male" }
                });

            migrationBuilder.InsertData(
                table: "AppointmentDetails",
                columns: new[] { "Id", "ConsultingDoctor", "Description", "NurseId", "PatientId", "PatientProblem", "ScheduleEndTime", "ScheduleStartTime", "Status" },
                values: new object[] { 1, "Physiotherapist", "Pain in hand & leg, SoreBody", 2, 4, "Accident induced damage", new DateTime(2024, 4, 8, 12, 12, 33, 72, DateTimeKind.Utc).AddTicks(2465), new DateTime(2024, 4, 8, 11, 12, 33, 72, DateTimeKind.Utc).AddTicks(2464), "Scheduled" });

            migrationBuilder.InsertData(
                table: "DoctorSpecialization",
                columns: new[] { "Id", "Specialization", "UserId" },
                values: new object[,]
                {
                    { 1, "BrainSurgery", 1 },
                    { 2, "Physiotherapist", 2 },
                    { 3, "EyeSpecialist", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_NurseId",
                table: "AppointmentDetails",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_PatientId",
                table: "AppointmentDetails",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSpecialization_UserId",
                table: "DoctorSpecialization",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentDetails");

            migrationBuilder.DropTable(
                name: "DoctorSpecialization");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
