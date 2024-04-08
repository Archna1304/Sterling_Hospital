using DataAccess_Layer.Models;

namespace DataAccess_Layer.Interface
{
    public interface IDoctorRepo
    {
        Task<List<AppointmentDetails>> GetAllAppointments();
        Task<bool> RescheduleAppointment(int appointmentId, DateTime newStartTime);
        Task<bool> CancelAppointment(int appointmentId);
        Task<bool> AssignDutyToNurse(int appointmentId, int nurseId);
    }
}
