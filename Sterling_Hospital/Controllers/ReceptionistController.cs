using DataAccess_Layer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.DTO;
using Service_Layer.Interface;
using Service_Layer.Services;

namespace Sterling_Hospital.Controllers
{
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistController : BaseController
    {

        #region prop
        private readonly IReceptionistService _receptionistService;
        private readonly IValidationService _validationService;
        #endregion

        #region Constructor
        public ReceptionistController(IReceptionistService receptionistService, IValidationService validationService)
        {
            _receptionistService = receptionistService;
            _validationService = validationService;
        }
        #endregion

        //API's

        #region Register Patient
        [HttpPost("CreatePatientProfile")]
        public async Task<IActionResult> CreatePatientProfile(RegisterPatientDTO registerPatientDTO)
        {
            var validationResponse = await _validationService.ValidatePatient(registerPatientDTO);
            if (validationResponse.Status != 200)
            {
                return BadRequest(validationResponse);
            }

            var response = await _receptionistService.CreatePatientProfile(registerPatientDTO);
            return StatusCode(response.Status, response);
        }
        #endregion

        #region Schedule Appointment
        [HttpPost("ScheduleAppointment")]
        public async Task<IActionResult> ScheduleAppointment([FromBody] AppointmentDTO appointmentDTO)
        {
            var validationResponse = await _validationService.ValidateAppointmentDTO(appointmentDTO);
            if (validationResponse.Status != 200)
            {
                return BadRequest(validationResponse);
            }

            var response = await _receptionistService.ScheduleAppointment(appointmentDTO);
            return StatusCode(response.Status, response);
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
