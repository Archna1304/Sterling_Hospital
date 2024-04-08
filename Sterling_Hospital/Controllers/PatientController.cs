using Microsoft.AspNetCore.Mvc;
using Service_Layer.Service;

namespace Sterling_Hospital.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase     
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/PatientAppointments/current/{patientId}
        [HttpGet("current/{patientId}")]
        public async Task<IActionResult> GetCurrentAppointments(int patientId)
        {
            var appointments = await _patientService.GetCurrentAppointments(patientId);
            return Ok(appointments);
        }

        // GET: api/PatientAppointments/previous/{patientId}
        [HttpGet("previous/{patientId}")]
        public async Task<IActionResult> GetPreviousAppointments(int patientId)
        {
            var appointments = await _patientService.GetPreviousAppointments(patientId);
            return Ok(appointments);
        }
    }
}
