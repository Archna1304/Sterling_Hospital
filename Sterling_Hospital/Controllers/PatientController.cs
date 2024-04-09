using Microsoft.AspNetCore.Mvc;
using Service_Layer.Service;

namespace Sterling_Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase     
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
        public async Task<IActionResult> GetCurrentAppointments(int patientId)
        {
            var appointments = await _patientService.GetCurrentAppointments(patientId);
            return Ok(appointments);
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
