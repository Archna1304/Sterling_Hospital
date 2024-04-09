using Service_Layer.DTO;

namespace Service_Layer.Interface
{
    public interface IAuthService
    {
        Task<ResponseDTO> Register(RegisterDTO registerDTO);
        Task<ResponseDTO> Login(LoginDTO loginDTO);
        Task<ResponseDTO> RegisterDoctor(RegisterDoctorDTO registerDoctorDTO);
    }
}
