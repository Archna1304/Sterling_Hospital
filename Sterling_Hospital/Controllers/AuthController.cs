using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;
using Service_Layer.Services;

namespace Sterling_Hospital.Controllers
{
    public class AuthController : BaseController
    {
        #region prop
        private readonly IAuthService _authService;
        private readonly IValidationService _validationService;
        #endregion

        #region Constructor
        public AuthController(IAuthService authService, IValidationService validationService)
        {
            _authService = authService;
            _validationService = validationService;
        }
        #endregion

        //API's

        #region Register User
        [Authorize(Roles = "Doctor")]
        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var validationResponse = await _validationService.ValidateUser(registerDTO);
            if (validationResponse.Status != 200)
            {
                return BadRequest(validationResponse);
            }

            var response = await _authService.Register(registerDTO);
            return StatusCode(response.Status, response);
        }
        #endregion

        #region Register Doctor
        [Authorize(Roles = "Doctor")]
        [HttpPost("RegisterDoctor")]
        public async Task<IActionResult> RegisterDoctor(RegisterDoctorDTO registerDoctorDTO)
        {
            var validationResponse = await _validationService.ValidateDoctor(registerDoctorDTO);
            if (validationResponse.Status != 200)
            {
                return BadRequest(validationResponse);
            }

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
            var validationResponse = await _validationService.ValidateLogin(loginDTO);
            if (validationResponse.Status != 200)
            {
                return BadRequest(validationResponse);
            }

            var response = await _authService.Login(loginDTO);
            return StatusCode(response.Status, response);
        }
        #endregion
    }
}
