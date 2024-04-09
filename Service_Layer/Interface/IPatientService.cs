using DataAccess_Layer.Models;

namespace Service_Layer.Interface
{
    public interface IPatientService
    {
        Task<AppointmentDetails> GetCurrentAppointment(int patientId);
        Task<List<AppointmentDetails>> GetPreviousAppointments(int patientId);
    }
}
