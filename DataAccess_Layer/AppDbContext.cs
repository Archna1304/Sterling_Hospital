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


        //Polymorphism
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

           
            

            base.OnModelCreating(modelBuilder);

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FirstName = "Archana",
                    LastName = "Vyas",
                    Email = "archana.vyas@bacancy.com",
                    PhoneNumber = "8401114826",
                    DateOfBirth = new DateTime(2002, 04, 13),
                    Sex = Sex.Female,
                    Address = "15-16 Bacancy",
                    PostalCode = "365541",
                    //use this to login Passwrod = "archana13"
                    Password = "5d7ad4424a19c1cbd4c8c1fe10d7fc344d5f3e40554d2d0ac842a83e06920a7f",
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
        }
    }
}
