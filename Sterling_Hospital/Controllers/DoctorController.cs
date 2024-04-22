using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;
using Service_Layer.Services;

namespace Sterling_Hospital.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : BaseController
    {
        #region Prop
        private readonly IDoctorService _doctorService;
        private readonly IValidationService _validationService;
        #endregion

        #region Constructor
        public DoctorController(IDoctorService doctorService, IValidationService validationService)
        {
            _doctorService = doctorService;
            _validationService = validationService;
        }
        #endregion

        //Methods

        #region All Doctors Appointment
        [HttpGet("Dashboard/AllAppointments")]
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
        [HttpPut("Dashboard/RescheduleAppointments")]
        public async Task<ActionResult<ResponseDTO>> RescheduleAppointment(ChangeAppointmentDTO changeAppointmentDTO)
        {
            try
            {
                var validationResponse = await _validationService.ValidateChangeAppointment(changeAppointmentDTO);
                if (validationResponse.Status != 200)
                {
                    return BadRequest(validationResponse);
                }

                var response = await _doctorService.RescheduleAppointment(changeAppointmentDTO);

                // Check the status of the response and return the corresponding ActionResult
                if (response.Status == 200)
                {
                    return Ok(response);
                }
                else 
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a StatusCode 500 with the error message
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while rescheduling appointment.", Error = ex.Message });
            }
        }
        #endregion

        #region Cancel Appointments
        [HttpDelete("Dashboard/CancelAppointments/{appointmentId}")]
        public async Task<ActionResult<ResponseDTO>> CancelAppointment(int appointmentId)
        {
            var response = await _doctorService.CancelAppointment(appointmentId);

            switch (response.Status)
            {
                case 200:
                    return Ok(new ResponseDTO { Status = 200, Message = "Appointment canceled successfully." });
                case 400:
                    return BadRequest(new ResponseDTO { Status = 400, Message = "Failed to cancel appointment. Appointment not found." });
                case 500:
                    return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while canceling appointment.", Error = response.Error });
                default:
                    // Handle any other unexpected status codes
                    return StatusCode(500, new ResponseDTO { Status = 500, Message = "An unexpected error occurred." });
            }
        }
        #endregion


        #region Assign nurse
        [HttpPost("AssignNurse")]
        public async Task<ActionResult<ResponseDTO>> AssignDutyToNurse(AssignNurseDTO assignNurseDTO)
        {
            try
            {
                var validationResponse = await _validationService.ValidateAssignNurse(assignNurseDTO);
                if (validationResponse.Status != 200)
                    return BadRequest(validationResponse);

                bool result = await _doctorService.AssignDutyToNurse(assignNurseDTO.AppointmentId, assignNurseDTO.NurseId);
                return result
                    ? Ok(new ResponseDTO { Status = 200, Message = "Duty assigned to nurse successfully." })
                    : BadRequest(new ResponseDTO { Status = 400, Message = "Failed to assign duty to nurse. Appointment not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDTO { Status = 500, Message = "An error occurred while assigning duty to nurse.", Error = ex.Message });
            }
        }
        #endregion


    }
}
