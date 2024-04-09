using DataAccess_Layer.Models;
using Service_Layer.DTO;

namespace Service_Layer.Interface
{
    public interface IReceptionistService
    {
        Task<ResponseDTO> ScheduleAppointment(AppointmentDTO appointmentDTO);
        Task<ResponseDTO> CreatePatientProfile(RegisterPatientDTO registerPatientDTO);
        Task<List<AppointmentDetails>> GetPatientAppointments(int patientId, DateTime? appointmentDate = null);

    }
}
