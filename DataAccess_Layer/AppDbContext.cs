using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess_Layer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> User { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecialization { get; set; }
        public DbSet<AppointmentDetails> AppointmentDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure enum to string conversion
            var StringSex = new EnumToStringConverter<Sex>();
            var StringRole = new EnumToStringConverter<Role>();
            var StringSpecialization = new EnumToStringConverter<Specialization>();
            var StringStatus = new EnumToStringConverter<Status>();

            modelBuilder.Entity<User>()
                .Property(e => e.Sex)
                .HasConversion(StringSex);

            modelBuilder.Entity<User>()
                .Property(e => e.Role)
                .HasConversion(StringRole);

            modelBuilder.Entity<DoctorSpecialization>()
                .Property(e => e.Specialization)
                .HasConversion(StringSpecialization);

            modelBuilder.Entity<AppointmentDetails>()
                .Property(e => e.Status)
                .HasConversion(StringStatus);

            modelBuilder.Entity<AppointmentDetails>()
            .HasOne(a => a.Nurse)
            .WithMany()
            .HasForeignKey(a => a.NurseId)
            .IsRequired(false);

            //modelBuilder.Entity<AppointmentDetails>()
            //.HasOne(a => a.Doctor)
            //.WithMany(u => u.NurseId)
            //.HasForeignKey(a => a.DoctorId)
            //.IsRequired();

            

            base.OnModelCreating(modelBuilder);

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "Archana",
                    LastName = "Vyas",
                    Email = "archana.vyas@bacancy.com",
                    PhoneNumber = "123-456-7890",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    Sex = Sex.Female,
                    Address = "123 Main St",
                    PostalCode = "12345",
                    Password = "password",
                    Role = Role.Doctor
                }
            );

            // Seed Doctor Specializations
            modelBuilder.Entity<DoctorSpecialization>().HasData(
                new DoctorSpecialization
                {
                    Id = 1,
                    UserId = 1,
                    Specialization = Specialization.BrainSurgery
                }
            );


            //// Seed Appointments
            //modelBuilder.Entity<AppointmentDetails>().HasData(
            //    new AppointmentDetails
            //    {
            //        Id = 1,
            //        PatientId = 4, // Linking to the User entity
            //        ScheduleStartTime = DateTime.UtcNow,
            //        ScheduleEndTime = DateTime.UtcNow.AddHours(1), // Example: 1 hour appointment
            //        PatientProblem = "Accident induced damage",
            //        Description = "Pain in hand & leg, SoreBody",
            //        Status = Status.Scheduled,
            //        ConsultingDoctor = Specialization.Physiotherapist.ToString(),
            //        NurseId = 2
            //    }
            //);
        }
    }
}
