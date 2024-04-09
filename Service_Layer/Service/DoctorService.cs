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
                bool isAvailable = await _doctorRepo.CheckAvailability(appointment.DoctorId ?? default(int), changeAppointmentDTO.NewConsultingDoctor, changeAppointmentDTO.NewAppointmentStartTime);

                if (!isAvailable)
                {
                    return new ResponseDTO { Status = 400, Message = "Doctor is not available at the requested time." };
                }

                // Update appointment details
                appointment.ScheduleStartTime = changeAppointmentDTO.NewAppointmentStartTime;
                appointment.ScheduleEndTime = changeAppointmentDTO.NewAppointmentEndTime;
                appointment.ConsultingDoctor = changeAppointmentDTO.NewConsultingDoctor;
                appointment.Status = DataAccess_Layer.Models.Status.Rescheduled;

                // Update appointment in the database
                bool result = await _doctorRepo.UpdateAppointment(appointment);

                if (result)
                {
                    // Send email notification
                    var emailDTO = new EmailDTO
                    {
                        Email = appointment.Patient.Email,
                        Subject = "Appointment Rescheduled",
                        Body = $"Dear {appointment.Patient.FirstName} {appointment.Patient.LastName},<br><br>Your appointment has been successfully rescheduled with {changeAppointmentDTO.NewConsultingDoctor} on {changeAppointmentDTO.NewAppointmentStartTime} to {changeAppointmentDTO.NewAppointmentEndTime}.<br><br>Regards,<br>Sterling Hospital"
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

                // Update appointment status to Cancelled
                appointment.Status = DataAccess_Layer.Models.Status.Cancelled;
                await _doctorRepo.UpdateAppointment(appointment);

                var emailDTO = new EmailDTO
                {
                    Email = appointment.Patient.Email,
                    Subject = "Appointment Cancellation",
                    Body = $"Dear {appointment.Patient.FirstName} {appointment.Patient.LastName},<br><br> Your appointment scheduled for {appointment.ScheduleStartTime} has been cancelled."
                };

                // Send the email
                _emailService.SendEmailAsync(emailDTO);

                return new ResponseDTO
                {
                    Status = 200,
                    Message = "Appointment cancelled successfully."
                };
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
