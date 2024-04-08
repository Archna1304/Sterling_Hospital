using DataAccess_Layer.Models;
using DataAccess_Layer.Repository;
using Service_Layer.Interface;

namespace Service_Layer.Service
{
    public class PatientService : IPatientService
    {
        private readonly PatientRepo _patientRepo;

        public PatientService(PatientRepo patientRepo)
        {
            _patientRepo = patientRepo;
        }

        // Get current appointments for a specific patient ordered by latest first
        public async Task<List<AppointmentDetails>> GetCurrentAppointments(int patientId)
        {
            return await _patientRepo.GetCurrentAppointments(patientId);
        }

        // Get previous appointments for a specific patient ordered by latest first
        public async Task<List<AppointmentDetails>> GetPreviousAppointments(int patientId)
        {
            return await _patientRepo.GetPreviousAppointments(patientId);
        }
    }
}
