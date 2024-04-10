using DataAccess_Layer.Models;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interface
{
    public interface IDoctorRepo
    {
        Task<List<object>> GetDoctorAppointments(string specialization, int doctorId);
        Task<string> RescheduleAppointment(int appointmentId, DateTime newStartTime, DateTime newEndTime);
        Task<string> CancelAppointment(int appointmentId);
        Task<bool> AssignDutyToNurse(int appointmentId, int nurseId);
        Task<bool> UpdateDoctorSpecialization(Specialization specialization, int userId);
        Task<int> CountDoctorsBySpecialization(Specialization specialization);
        Task<bool> CheckAvailability(int doctorId, DateTime appointmentTime);
        Task<AppointmentDetails> GetAppointmentById(int appointmentId);
       
    }
}
