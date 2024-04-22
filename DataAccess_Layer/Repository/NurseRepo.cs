using DataAccess_Layer.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Layer.Repository
{
    public class NurseRepo : INurseRepo
    {
        #region prop
        private readonly AppDbContext _context;
        #endregion

        #region constuctor
        public NurseRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion


        //Methods

        #region Get Duties
        public async Task<List<dynamic>> GetNurseDuties(int nurseId)
        {
            // LINQ query to fetch nurse duties based on nurse ID
            return await _context.AppointmentDetails
                .Where(u => u.NurseId == nurseId)
                .OrderByDescending(a => a.ScheduleStartTime)
                .Select(u => new
                {
                    // Projection to select specific fields for each nurse duty
                    patientId = "Sterling_" + u.Patient.UserId.ToString(),
                    patientName = u.Patient.FirstName + " " + u.Patient.LastName,
                    gender = u.Patient.Sex.ToString(),
                    Age = DateTime.UtcNow.Year - EF.Functions.DateDiffYear(u.Patient.DateOfBirth, DateTime.UtcNow),
                    phoneNumber = u.Patient.PhoneNumber,
                    doctor = u.DoctorId,
                    doctorSpecialization = u.ConsultingDoctor,
                    scheduleStartTime = u.ScheduleStartTime,
                    scheduleEndTime = u.ScheduleEndTime,
                    patientProblem = u.PatientProblem,
                    description = u.Description,
                })
                .ToListAsync<dynamic>();
        }

        #endregion
    }
}
