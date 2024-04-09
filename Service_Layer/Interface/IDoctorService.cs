using Service_Layer.DTO;

namespace Service_Layer.Interface
{
    public interface IDoctorService
    {
        Task<List<dynamic>> GetDoctorAppointments(string specialization, int doctorId);
        Task<ResponseDTO> RescheduleAppointment(ChangeAppointmentDTO changeAppointmentDTO);
        Task<bool> CancelAppointment(int appointmentId);
        Task<bool> AssignDutyToNurse(int appointmentId, int nurseId);
    }
}
