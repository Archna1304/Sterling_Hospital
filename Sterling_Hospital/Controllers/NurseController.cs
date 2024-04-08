using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;

namespace Sterling_Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NurseController : ControllerBase
    {
        private readonly INurseService _nurseService;

        public NurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }

        [HttpGet("NurseDuties")]
        public async Task<ActionResult<List<AppointmentDetails>>> GetNurseDuties()
        {
            try
            {
                var duties = await _nurseService.GetAllDuties();
                return Ok(duties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while fetching nurse duties.", Error = ex.Message });
            }
        }
    }
}
