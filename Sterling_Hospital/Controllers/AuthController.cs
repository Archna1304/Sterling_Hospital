using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;



namespace Sterling_Hospital.Controllers
{

    public class AuthController : BaseController
    {
        #region prop
        private readonly IAuthService _authService;
        #endregion

        #region Constructor
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        //API's

        #region Register User
        //[Authorize(Roles = "Doctor")]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var response = await _authService.Register(registerDTO);
            return StatusCode(response.Status, response);
        }
        #endregion

        #region Register Doctor
        //[Authorize(Roles = "Doctor")]
        [HttpPost("RegisterDoctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorDTO registerDoctorDTO)
        {
            var response = await _authService.RegisterDoctor(registerDoctorDTO);

            if (response.Status == 200)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(response.Status, response);
            }
        }
        #endregion

        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var response = await _authService.Login(loginDTO);
            return StatusCode(response.Status, response);
        }
        #endregion
    }
}
