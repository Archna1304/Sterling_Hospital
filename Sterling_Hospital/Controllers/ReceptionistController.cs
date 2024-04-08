using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;
using Service_Layer.Service;

namespace Sterling_Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionistController : ControllerBase
    {

        #region prop
        private readonly IReceptionistService _receptionistService;
        #endregion

        #region Constructor
        public ReceptionistController(IReceptionistService receptionistService)
        {
            _receptionistService = receptionistService;
        }
        #endregion

        //API's

        #region Register Patient
        [HttpPost("CreatePatientProfile")]
        public async Task<IActionResult> CreatePatientProfile(RegisterPatientDTO registerPatientDTO)
        {
            var response = await _receptionistService.CreatePatientProfile(registerPatientDTO);
            return StatusCode(response.Status, response);
        }
        #endregion

        #region Schedule Appointment
        [HttpPost("ScheduleAppointment")]
        public async Task<IActionResult> ScheduleAppointment([FromBody] AppointmentDTO appointmentDTO)
        {
            var response = await _receptionistService.ScheduleAppointment(appointmentDTO);
            return StatusCode(response.Status, response);
        }
        #endregion

        #region Change Appointment

        [HttpPost("ChangeAppointment")]
        public async Task<IActionResult> ChangeAppointment([FromBody] ChangeAppointmentDTO changeAppointmentDTO)
        {
            var response = await _receptionistService.ChangeAppointment(changeAppointmentDTO);

            if (response.Status == 200)
            {
                return Ok(response); // Return 200 OK status with response
            }
            else
            {
                return BadRequest(response); // Return 400 Bad Request status with response
            }
        }


        #endregion

        #region Get Patient Appointment

        [HttpGet("GetPatientAppointments/{patientId}")]
        public async Task<ActionResult<List<AppointmentDetails>>> GetPatientAppointments(int patientId, DateTime? appointmentDate = null)
        {
            try
            {
                var appointments = await _receptionistService.GetPatientAppointments(patientId, appointmentDate);
                if (appointments == null)
                {
                    return NotFound("No appointments found for the specified patient.");
                }
                return Ok(appointments);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        #endregion
    }


}
