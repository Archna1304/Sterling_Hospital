using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;

namespace Sterling_Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet("AllAppointments")]
        public async Task<ActionResult<List<AppointmentDetails>>> GetAllAppointments()
        {
            try
            {
                var appointments = await _doctorService.GetAllAppointments();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while fetching appointments.", Error = ex.Message });
            }
        }

        [HttpPut("RescheduleAppointments/{appointmentId}")]
        public async Task<ActionResult<ResponseDTO>> RescheduleAppointment(int appointmentId, ChangeAppointmentDTO changeAppointmentDTO)
        {
            try
            {
                bool result = await _doctorService.RescheduleAppointment(appointmentId, changeAppointmentDTO.NewAppointmentStartTime);
                if (result)
                    return Ok(new ResponseDTO { Status = 200, Message = "Appointment rescheduled successfully." });
                else
                    return BadRequest(new ResponseDTO { Status = 400, Message = "Failed to reschedule appointment. Appointment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while rescheduling appointment.", Error = ex.Message });
            }
        }

        [HttpDelete("CancelAppointments/{appointmentId}")]
        public async Task<ActionResult<ResponseDTO>> CancelAppointment(int appointmentId)
        {
            try
            {
                bool result = await _doctorService.CancelAppointment(appointmentId);
                if (result)
                    return Ok(new ResponseDTO { Status = 200, Message = "Appointment canceled successfully." });
                else
                    return BadRequest(new ResponseDTO { Status = 400, Message = "Failed to cancel appointment. Appointment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while canceling appointment.", Error = ex.Message });
            }
        }

        [HttpPost("AssignNurse/{appointmentId}")]
        public async Task<ActionResult<ResponseDTO>> AssignDutyToNurse(int appointmentId, AssignNurseDTO assignNurseDTO)
        {
            try
            {
                bool result = await _doctorService.AssignDutyToNurse(appointmentId, assignNurseDTO.NurseId);
                if (result)
                    return Ok(new ResponseDTO { Status = 200, Message = "Duty assigned to nurse successfully." });
                else
                    return BadRequest(new ResponseDTO { Status = 400, Message = "Failed to assign duty to nurse. Appointment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while assigning duty to nurse.", Error = ex.Message });
            }
        }
    }
}
