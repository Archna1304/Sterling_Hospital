using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;

namespace Sterling_Hospital.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class NurseController : BaseController
    {
        #region prop
        private readonly INurseService _nurseService;
        #endregion

        #region Constructor
        public NurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }
        #endregion

        //Methods

        #region Nurse Duties
        [HttpGet("NurseDuties")]
        public async Task<ActionResult<List<AppointmentDetails>>> GetNurseDuties(int nurseId)
        {
            try
            {
                var duties = await _nurseService.GetNurseDuties(nurseId);
                return Ok(duties);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while fetching nurse duties.", Error = ex.Message });
            }
        }
        #endregion
    }
}
