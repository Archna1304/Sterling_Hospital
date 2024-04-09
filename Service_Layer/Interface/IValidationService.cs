using Service_Layer.DTO;

namespace Service_Layer.Services
{
    public interface IValidationService
    {
        Task<ValidationErrorDTO> ValidateUser(RegisterDTO registerDTO);
    }
}
