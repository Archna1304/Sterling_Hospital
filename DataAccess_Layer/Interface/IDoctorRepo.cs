using DataAccess_Layer.Models;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interface
{
    public interface IDoctorRepo
    {
        Task<List<object>> GetDoctorAppointments(string specialization, int doctorId);
        Task<bool> RescheduleAppointment(int appointmentId, DateTime newStartTime, DateTime newEndTime, string newConsultingDoctor);
        Task<bool> CancelAppointment(int appointmentId);
        Task<bool> AssignDutyToNurse(int appointmentId, int nurseId);
        Task<bool> UpdateDoctorSpecialization(Specialization specialization, int userId);
        Task<bool> CheckAvailability(int doctorId, string consultingDoctor, DateTime appointmentTime);
        Task<AppointmentDetails> GetAppointmentById(int appointmentId);
        Task<bool> UpdateAppointment(AppointmentDetails appointment);
    }
}
