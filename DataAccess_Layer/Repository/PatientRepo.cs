using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Layer.Repository
{
    public class PatientRepo : IPatientRepo
    {
        #region prop
        private readonly AppDbContext _context;
        #endregion

        #region constructor 
        public PatientRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        //Methods

        #region Get Current Appointment
        // Get the most recent appointment for a specific patient
        public async Task<AppointmentDetails> GetCurrentAppointments(int patientId)
        {
            return await _context.AppointmentDetails
                .Where(a => a.PatientId == patientId && a.ScheduleStartTime >= DateTime.Now)
                .OrderByDescending(a => a.ScheduleStartTime)
                .FirstOrDefaultAsync();
        }
        #endregion

        #region Get Appointment History

        // Get previous appointments for a specific patient ordered by latest first
        public async Task<List<AppointmentDetails>> GetPreviousAppointments(int patientId)
        {
            return await _context.AppointmentDetails
                .Where(a => a.PatientId == patientId && a.ScheduleStartTime < DateTime.Now)
                .OrderByDescending(a => a.ScheduleStartTime)
                .ToListAsync();
        }
        #endregion
    }
}
