using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service_Layer.Service;

namespace Sterling_Hospital.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : BaseController     
    {

        #region Prop
        private readonly PatientService _patientService;
        #endregion

        #region Constructor
        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }
        #endregion

        //Method

        #region Current Appointments

        // GET: api/PatientAppointments/current/{patientId}
        [HttpGet("CurrentAppointment/{patientId}")]
        public async Task<IActionResult> GetCurrentAppointment(int patientId)
        {
            var appointment = await _patientService.GetCurrentAppointment(patientId);
            if (appointment == null)
            {
                return NotFound("No current appointments found for the specified patient.");
            }
            return Ok(appointment);
        }
        #endregion

        #region Appointment History
        // GET: api/PatientAppointments/previous/{patientId}
        [HttpGet("AppointmentHistory/{patientId}")]
        public async Task<IActionResult> GetPreviousAppointments(int patientId)
        {
            var appointments = await _patientService.GetPreviousAppointments(patientId);
            return Ok(appointments);
        }
        #endregion
    }
}
