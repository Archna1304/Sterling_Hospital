using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Identity;
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
            return !(await _context.AppointmentDetails
                .AnyAsync(a => a.DoctorId == doctorId && a.ScheduleStartTime <= appointmentTime && a.ScheduleEndTime >= appointmentTime));
        }

        #endregion

        #region Reschedule Appointment
        public async Task<bool> RescheduleAppointment(int appointmentId, DateTime newStartTime, string newConsultingDoctor)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.ScheduleStartTime = newStartTime;
                appointment.ConsultingDoctor = newConsultingDoctor; // Update consulting doctor
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion


        #region Cancel Appointment
        public async Task<bool> CancelAppointment(int appointmentId)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
            if (appointment != null)
            {
                _context.AppointmentDetails.Remove(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
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
        #endregion

    }
}
