using DataAccess_Layer.Interface;
using Service_Layer.DTO;
using Service_Layer.Interface;

namespace Service_Layer.Service
{
    public class DoctorService : IDoctorService
    {
        #region Prop
        private readonly IDoctorRepo _doctorRepo;
        private readonly IEmailService _emailService;
        #endregion

        #region constructor
        public DoctorService(IDoctorRepo doctorRepo, IEmailService emailService)
        {
            _doctorRepo = doctorRepo;
            _emailService = emailService;
        }
        #endregion

        //Methods

        #region Get Doctor Appointments
        public async Task<List<dynamic>> GetDoctorAppointments(string specialization, int doctorId)
        {
            return await _doctorRepo.GetDoctorAppointments(specialization, doctorId);
        }
        #endregion

        #region Reschedule Appointment
        public async Task<ResponseDTO> RescheduleAppointment(ChangeAppointmentDTO changeAppointmentDTO)
        {
            try
            {
                // Check if the appointment exists
                var appointment = await _doctorRepo.GetAppointmentById(changeAppointmentDTO.AppointmentId);
                if (appointment == null)
                {
                    return new ResponseDTO { Status = 400, Message = "Appointment not found." };
                }

                // Check availability for the new appointment time
                bool isAvailable = await _doctorRepo.CheckAvailability(appointment.DoctorId ?? default(int), changeAppointmentDTO.NewAppointmentStartTime);

                if (!isAvailable)
                {
                    return new ResponseDTO { Status = 400, Message = "Doctor is not available at the requested time." };
                }

                // Reschedule the appointment
                string patientEmail = await _doctorRepo.RescheduleAppointment(changeAppointmentDTO.AppointmentId, changeAppointmentDTO.NewAppointmentStartTime, changeAppointmentDTO.NewAppointmentEndTime);

                if (patientEmail != null)
                {
                    // Send email notification
                    var emailDTO = new EmailDTO
                    {
                        Email = patientEmail,
                        Subject = "Appointment Rescheduled",
                        Body = $"Dear {appointment.Patient.FirstName} {appointment.Patient.LastName},<br><br>Your appointment has been successfully rescheduled on {changeAppointmentDTO.NewAppointmentStartTime} to {changeAppointmentDTO.NewAppointmentEndTime}.<br><br>Regards,<br>Sterling Hospital"
                    };
                    _emailService.SendEmailAsync(emailDTO);

                    return new ResponseDTO { Status = 200, Message = "Appointment rescheduled successfully." };
                }
                else
                {
                    return new ResponseDTO { Status = 400, Message = "Failed to reschedule appointment. Appointment not found." };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Message = "An error occurred while rescheduling appointment.", Error = ex.Message };
            }
        }

        #endregion

        #region Cancel Appointments
        public async Task<ResponseDTO> CancelAppointment(int appointmentId)
        {
            try
            {
                // Check if the appointment exists
                var appointment = await _doctorRepo.GetAppointmentById(appointmentId);
                if (appointment == null)
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Appointment not found."
                    };
                }

                // Cancel the appointment
                string patientEmail = await _doctorRepo.CancelAppointment(appointmentId);

                if (patientEmail != null)
                {
                    var emailDTO = new EmailDTO
                    {
                        Email = patientEmail,
                        Subject = "Appointment Cancellation",
                        Body = $"Dear {appointment.Patient.FirstName} {appointment.Patient.LastName},<br><br>Your appointment scheduled for {appointment.ScheduleStartTime} has been cancelled."
                    };

                    // Send the email
                    _emailService.SendEmailAsync(emailDTO);

                    return new ResponseDTO
                    {
                        Status = 200,
                        Message = "Appointment cancelled successfully."
                    };
                }
                else
                {
                    return new ResponseDTO
                    {
                        Status = 400,
                        Message = "Failed to cancel appointment. Appointment not found."
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO
                {
                    Status = 500,
                    Message = "An error occurred while cancelling appointment.",
                    Error = ex.Message
                };
            }
        }
        #endregion



        #region AsisgnDuty to Nurse
        public async Task<bool> AssignDutyToNurse(int appointmentId, int nurseId)
        {
            return await _doctorRepo.AssignDutyToNurse(appointmentId, nurseId);
        }
        #endregion
    }
}
