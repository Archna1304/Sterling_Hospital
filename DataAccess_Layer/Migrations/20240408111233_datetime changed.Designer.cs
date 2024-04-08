﻿// <auto-generated />
using System;
using DataAccess_Layer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240408111233_datetime changed")]
    partial class datetimechanged
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccess_Layer.Models.AppointmentDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ConsultingDoctor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NurseId")
                        .HasColumnType("int");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<string>("PatientProblem")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ScheduleEndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ScheduleStartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("NurseId");

                    b.HasIndex("PatientId");

                    b.ToTable("AppointmentDetails");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConsultingDoctor = "Physiotherapist",
                            Description = "Pain in hand & leg, SoreBody",
                            NurseId = 2,
                            PatientId = 4,
                            PatientProblem = "Accident induced damage",
                            ScheduleEndTime = new DateTime(2024, 4, 8, 12, 12, 33, 72, DateTimeKind.Utc).AddTicks(2465),
                            ScheduleStartTime = new DateTime(2024, 4, 8, 11, 12, 33, 72, DateTimeKind.Utc).AddTicks(2464),
                            Status = "Scheduled"
                        });
                });

            modelBuilder.Entity("DataAccess_Layer.Models.DoctorSpecialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DoctorSpecialization");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Specialization = "BrainSurgery",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Specialization = "Physiotherapist",
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            Specialization = "EyeSpecialist",
                            UserId = 3
                        });
                });

            modelBuilder.Entity("DataAccess_Layer.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Password");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Address = "123 Main St",
                            DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "archana.vyas@bacancy.com",
                            FirstName = "Archana",
                            LastName = "Vyas",
                            Password = "password",
                            PhoneNumber = "123-456-7890",
                            PostalCode = "12345",
                            Role = "Doctor",
                            Sex = "Female"
                        },
                        new
                        {
                            UserId = 2,
                            Address = "123 Main St",
                            DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "parangi.rathod@bacancy.com",
                            FirstName = "Parangi",
                            LastName = "Rathod",
                            Password = "password",
                            PhoneNumber = "123-456-7890",
                            PostalCode = "12345",
                            Role = "Doctor",
                            Sex = "Female"
                        },
                        new
                        {
                            UserId = 3,
                            Address = "123 Main St",
                            DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "khushbu.oza@bacancy.com",
                            FirstName = "Khushbu",
                            LastName = "Oza",
                            Password = "password",
                            PhoneNumber = "123-456-7890",
                            PostalCode = "12345",
                            Role = "Doctor",
                            Sex = "Female"
                        },
                        new
                        {
                            UserId = 4,
                            Address = "123 Main St",
                            DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "kishan.prajapati@bacancy.com",
                            FirstName = "Kishan",
                            LastName = "Prajapati",
                            Password = "password",
                            PhoneNumber = "123-456-7890",
                            PostalCode = "12345",
                            Role = "Patient",
                            Sex = "Male"
                        });
                });

            modelBuilder.Entity("DataAccess_Layer.Models.AppointmentDetails", b =>
                {
                    b.HasOne("DataAccess_Layer.Models.User", "Nurse")
                        .WithMany()
                        .HasForeignKey("NurseId");

                    b.HasOne("DataAccess_Layer.Models.User", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Nurse");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("DataAccess_Layer.Models.DoctorSpecialization", b =>
                {
                    b.HasOne("DataAccess_Layer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}