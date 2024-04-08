using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;



namespace Sterling_Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
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

        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var response = await _authService.Register(registerDTO);
            return StatusCode(response.Status, response);
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
