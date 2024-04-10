using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
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
                    NurseId = table.Column<int>(type: "int", nullable: true),
                    DoctorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppointmentDetails_User_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "User",
                        principalColumn: "UserId");
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
                values: new object[] { 1, "15-16 Bacancy", new DateTime(2002, 4, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "archana.vyas@bacancy.com", "Archana", "Vyas", "5d7ad4424a19c1cbd4c8c1fe10d7fc344d5f3e40554d2d0ac842a83e06920a7f", "8401114826", "365541", "Doctor", "Female" });

            migrationBuilder.InsertData(
                table: "DoctorSpecialization",
                columns: new[] { "Id", "Specialization", "UserId" },
                values: new object[] { 1, "BrainSurgery", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetails_DoctorId",
                table: "AppointmentDetails",
                column: "DoctorId");

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
