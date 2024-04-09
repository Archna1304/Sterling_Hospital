using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Service_Layer.Interface;

namespace Service_Layer.Service
{
    public class PatientService : IPatientService
    {
        #region prop
        private readonly IPatientRepo _patientRepo;
        #endregion

        #region Constructor
        public PatientService(IPatientRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }

        #endregion


        //Methods

        #region Get Current Appointment
        // Get current appointments for a specific patient ordered by latest first
        public async Task<List<AppointmentDetails>> GetCurrentAppointments(int patientId)
        {
            return await _patientRepo.GetCurrentAppointments(patientId);
        }
        #endregion

        #region Get Appointment History
        // Get previous appointments for a specific patient ordered by latest first
        public async Task<List<AppointmentDetails>> GetPreviousAppointments(int patientId)
        {
            return await _patientRepo.GetPreviousAppointments(patientId);
        }
        #endregion
    }
}
