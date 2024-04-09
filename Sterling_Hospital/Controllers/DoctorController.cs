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
        #region Prop
        private readonly IDoctorService _doctorService;
        #endregion

        #region Constructor
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        #endregion

        //Methods

        #region All Doctors Appointment
        [HttpGet("AllAppointments")]
        public async Task<ActionResult<List<AppointmentDetails>>> GetDoctorAppointments(string specialization, int doctorId)
        {
            try
            {
                var appointments = await _doctorService.GetDoctorAppointments(specialization, doctorId);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while fetching appointments.", Error = ex.Message });
            }
        }
        #endregion

        #region Reschedule Appointment
        [HttpPut("RescheduleAppointments")]
        public async Task<ActionResult<ResponseDTO>> RescheduleAppointment(ChangeAppointmentDTO changeAppointmentDTO)
        {
            try
            {
                var response = await _doctorService.RescheduleAppointment(changeAppointmentDTO);

                // Check the status of the response and return the corresponding ActionResult
                if (response.Status == 200)
                {
                    return Ok(response);
                }
                else if (response.Status == 400)
                {
                    return BadRequest(response);
                }
                else
                {
                    return StatusCode(500, response);
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a StatusCode 500 with the error message
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while rescheduling appointment.", Error = ex.Message });
            }
        }
        #endregion

        #region Cancel Appointment
        [HttpDelete("CancelAppointments/{appointmentId}")]
        public async Task<ActionResult<ResponseDTO>> CancelAppointment(int appointmentId)
        {
            try
            {
                bool result = await _doctorService.CancelAppointment(appointmentId);
                return result ? Ok(new ResponseDTO { Status = 200, Message = "Appointment canceled successfully." }) :
                                BadRequest(new ResponseDTO { Status = 400, Message = "Failed to cancel appointment. Appointment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while canceling appointment.", Error = ex.Message });
            }
        }
        #endregion

        #region Assign nurse

        [HttpPost("AssignNurse")]
        public async Task<ActionResult<ResponseDTO>> AssignDutyToNurse(AssignNurseDTO assignNurseDTO)
        {
            try
            {
                bool result = await _doctorService.AssignDutyToNurse(assignNurseDTO.AppointmentId, assignNurseDTO.NurseId);
                return result ? Ok(new ResponseDTO { Status = 200, Message = "Duty assigned to nurse successfully." }) :
                               BadRequest(new ResponseDTO { Status = 400, Message = "Failed to assign duty to nurse. Appointment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while assigning duty to nurse.", Error = ex.Message });
            }
        }

        #endregion

    }
}
