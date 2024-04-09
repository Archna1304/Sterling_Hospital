using Service_Layer.DTO;

namespace Service_Layer.Services
{
    public interface IValidationService
    {
        Task<ValidationErrorDTO> ValidateUser(RegisterDTO registerDTO);
        Task<ValidationErrorDTO> ValidateDoctor(RegisterDoctorDTO registerDoctorDTO);
        Task<ValidationErrorDTO> ValidatePatient(RegisterPatientDTO registerPatientDTO);
        Task<ValidationErrorDTO> ValidateLogin(LoginDTO loginDTO);
        Task<ValidationErrorDTO> ValidateEmail(EmailDTO emailDTO);
        Task<ValidationErrorDTO> ValidateChangeAppointment(ChangeAppointmentDTO changeAppointmentDTO);
        Task<ValidationErrorDTO> ValidateAssignNurse(AssignNurseDTO assignNurseDTO);
        Task<ValidationErrorDTO> ValidateAppointmentDTO(AppointmentDTO appointmentDTO);

    }
}
