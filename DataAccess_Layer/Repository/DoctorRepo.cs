using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Layer.Repository
{
    public class DoctorRepo : IDoctorRepo
    {
        #region Prop
        private readonly AppDbContext _context;
        #endregion

        #region construtor
        public DoctorRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        //Methods

        #region Get Doctor Appointments
        public async Task<List<dynamic>> GetDoctorAppointments(string specialization, int doctorId)
        {
            var appointments = await _context.AppointmentDetails
                .Where(a => a.ConsultingDoctor == specialization && a.DoctorId == doctorId)
                .OrderByDescending(a => a.ScheduleStartTime)
                .Select(a => new
                {
                    patientId = "Sterling_" + a.Patient.UserId.ToString(),
                    patientName = a.Patient.FirstName + " " + a.Patient.LastName,
                    gender = a.Patient.Sex.ToString(),
                    email = a.Patient.Email,
                    phoneNumber = a.Patient.PhoneNumber,
                    scheduleStartTime = a.ScheduleStartTime,
                    scheduleEndTime = a.ScheduleEndTime,
                    patientProblem = a.PatientProblem,
                    description = a.Description,
                    status = a.Status.ToString()
                })
                .ToListAsync();

            return appointments.Cast<dynamic>().ToList();
        }
        #endregion

        #region Check Availability
        public async Task<bool> CheckAvailability(int doctorId, DateTime appointmentTime)
        {
            // Your implementation to check availability in the database
            return !(await _context.AppointmentDetails
                .AnyAsync(a => a.DoctorId == doctorId && a.ScheduleStartTime <= appointmentTime && a.ScheduleEndTime >= appointmentTime));
        }
        #endregion

        #region Reschedule Appointment
        public async Task<string> RescheduleAppointment(int appointmentId, DateTime newStartTime, DateTime newEndTime)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
            int patientId = appointment.PatientId;
            if (appointment != null)
            {
                appointment.Status = Status.Rescheduled;
                appointment.ScheduleStartTime = newStartTime;
                appointment.ScheduleEndTime = newEndTime;
                _context.Update(appointment);
                await _context.SaveChangesAsync();

                var patient = await _context.User.FirstOrDefaultAsync(u => u.UserId == patientId);
                var email = patient.Email;
                return email;
            }
            return null;
        }
        #endregion

        #region Get Appointment by ID
        public async Task<AppointmentDetails> GetAppointmentById(int appointmentId)
        {
            return await _context.AppointmentDetails.FindAsync(appointmentId);
        }
        #endregion

       

        #region Cancel Appointment
        public async Task<string> CancelAppointment(int appointmentId)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
            int patientId = appointment.PatientId;
            if (appointment != null)
            {
                appointment.Status = Status.Cancelled;
                _context.Update(appointment);
                await _context.SaveChangesAsync();

                var patient = await _context.User.FirstOrDefaultAsync(u => u.UserId == patientId);
                var email = patient.Email;
                return email;
            }
            return null;
        }
        #endregion


        #region Assign Duty to Nurse
        public async Task<bool> AssignDutyToNurse(int appointmentId, int nurseId)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.NurseId = nurseId;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion

        //Additional Method

        #region Doctor Specialization
        public async Task<bool> UpdateDoctorSpecialization(Specialization specialization, int userId)
        {
            try
            {
                // Check if the doctor already has a specialization
                var existingSpecialization = await _context.DoctorSpecialization.FirstOrDefaultAsync(ds => ds.UserId == userId);

                // If the doctor already has a specialization, update it
                if (existingSpecialization != null)
                {
                    existingSpecialization.Specialization = specialization;
                    _context.DoctorSpecialization.Update(existingSpecialization);
                }
                else
                {
                    // Otherwise, create a new DoctorSpecialization entry
                    var newSpecialization = new DoctorSpecialization
                    {
                        UserId = userId,
                        Specialization = specialization
                    };
                    _context.DoctorSpecialization.Add(newSpecialization);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return false;
            }

        }


        public async Task<int> CountDoctorsBySpecialization(Specialization specialization)
        {
            return await _context.DoctorSpecialization
                .Where(ds => ds.Specialization == specialization)
                .CountAsync();
        }



        #endregion




    }
}
