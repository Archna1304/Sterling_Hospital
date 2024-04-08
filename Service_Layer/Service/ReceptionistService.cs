using AutoMapper;
using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.Extensions.Configuration;
using Service_Layer.DTO;
using Service_Layer.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Service_Layer.Service
{

    public class ReceptionistService : IReceptionistService
    {

        #region prop
        private readonly IReceptionistRepo _receptionistRepo;
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ReceptionistService(IReceptionistRepo receptionistRepo, IConfiguration configuration, IEmailService emailService, IUserRepo userRepo, IMapper mapper)
        {
            _receptionistRepo = receptionistRepo;
            _configuration = configuration;
            _emailService = emailService;
            _userRepo = userRepo;
            _mapper = mapper;
        }
        #endregion

        // Method


        #region Create Patient Profile 
        public async Task<ResponseDTO> CreatePatientProfile(RegisterPatientDTO registerPatientDTO)
        {
            try
            {
                string password;

                // Check if the patient already exists
                var existingPatient = await _receptionistRepo.CheckPatient(registerPatientDTO.FirstName, registerPatientDTO.Email, registerPatientDTO.DateOfBirth);
                if (existingPatient)
                {
                    // If Patient already exists
                    return new ResponseDTO { Status = 400, Message = "Patient with this profile already exists." };
                }

                // For patients, password format is FirstNameBirthDate
                password = CreatePasswordHash(registerPatientDTO.FirstName + registerPatientDTO.DateOfBirth.ToString("yyyyMMdd"));

                // Hash the password
                string passwordHash = CreatePasswordHash(password);

                // Map RegisterPatientDTO to User entity
                var patient = _mapper.Map<User>(registerPatientDTO);

                // Generate patient ID
                int patientNumber = await _receptionistRepo.GetNextPatientNumber();
                string patientId = $"Sterling_{patientNumber}";

                // Set additional properties
                patient.Password = passwordHash;
                patient.Role = DataAccess_Layer.Models.Role.Patient;

                // Add the patient to the database
                bool registered = await _receptionistRepo.CreatePatientProfile(patient);

                if (registered)
                {
                    string fullName = $"{patient.FirstName} {patient.LastName}";
                    // Send email to the patient
                    var emailDTO = new EmailDTO
                    {
                        Email = patient.Email,
                        Subject = "Patient Profile Created Successfully",
                        Body = $"Dear {fullName},<br><br> Your patient profile at Sterling Hospital has been created successfully.<br><br> Your patient ID is: {patientId} and your password is: {registerPatientDTO.Password}<br><br>Regards,<br>Sterling Hospital"
                    };

                    _emailService.SendEmailAsync(emailDTO);

                    return new ResponseDTO { Status = 200, Message = "Patient profile created successfully. Email sent to patient." };
                }
                else
                {
                    return new ResponseDTO { Status = 500, Message = "Failed to create patient profile." };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Message = "An error occurred while creating patient profile.", Error = ex.Message };
            }
        }
        #endregion

        #region Schedule Appointment

        public async Task<ResponseDTO> ScheduleAppointment(AppointmentDTO appointmentDTO)
        {
            try
            {
                // Check if the patient exists
                bool patientExists = await _receptionistRepo.CheckPatientById(appointmentDTO.PatientId);
                if (!patientExists)
                {
                    return new ResponseDTO { Status = 400, Message = "Patient does not exist." };
                }

                // Check doctor availability
                bool doctorAvailable = await _receptionistRepo.CheckDoctorAvailability((Specialization)Enum.Parse(typeof(Specialization), appointmentDTO.ConsultingDoctor), appointmentDTO.ScheduleStartTime);
                if (!doctorAvailable)
                {
                    return new ResponseDTO { Status = 400, Message = "Doctor is not available at the specified time." };
                }



                // Fetch the user object based on the userId (patientId)
                var user = await _receptionistRepo.GetUserById(appointmentDTO.PatientId);
                if (user == null)
                {
                    return new ResponseDTO { Status = 400, Message = "Patient details not found." };
                }

                // Schedule the appointment
                var appointmentDetails = _mapper.Map<AppointmentDetails>(appointmentDTO);

                bool appointmentScheduled = await _receptionistRepo.ScheduleAppointment(appointmentDetails);
                if (appointmentScheduled)
                {
                    // Send email to the patient
                    var emailDTO = new EmailDTO
                    {
                        Email = user.Email,
                        Subject = "Appointment Scheduled Successfully",
                        Body = $"Dear {user.FirstName} {user.LastName},<br><br>Your appointment has been scheduled successfully.<br><br>Appointment Date: {appointmentDTO.ScheduleStartTime}<br>Consulting Doctor: {appointmentDTO.ConsultingDoctor}<br><br>Regards,<br>Sterling Hospital"
                    };
                    _emailService.SendEmailAsync(emailDTO);

                    return new ResponseDTO { Status = 200, Message = "Appointment scheduled successfully. Email sent to patient." };
                }
                else
                {
                    return new ResponseDTO { Status = 500, Message = "Failed to schedule appointment." };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return new ResponseDTO { Status = 500, Message = "An error occurred while scheduling appointment.", Error = ex.Message };
            }
        }


        #endregion

        #region Get Patient Appointment Details

        public async Task<List<AppointmentDetails>> GetPatientAppointments(int patientId, DateTime? appointmentDate = null)
        {
            try
            {
                var appointments = await _receptionistRepo.GetPatientAppointments(patientId, appointmentDate);
                return appointments;
            }
            catch (Exception)
            {
                // Handle exceptions
                return null;
            }
        }


        #endregion

        #region Change Appointment

        public async Task<ResponseDTO> ChangeAppointment(ChangeAppointmentDTO changeAppointmentDTO)
        {
            try
            {
                // Check doctor availability for the new appointment time
                bool doctorAvailable = await _receptionistRepo.CheckDoctorAvailability((Specialization)Enum.Parse(typeof(Specialization), changeAppointmentDTO.NewConsultingDoctor), changeAppointmentDTO.NewAppointmentStartTime);

                if (!doctorAvailable)
                {
                    // Doctor is not available at the new time
                    return new ResponseDTO { Status = 400, Message = "Doctor is not available at the specified time." };
                }

                // Call the repository method to get the appointment details
                var appointments = await _receptionistRepo.GetPatientAppointments(changeAppointmentDTO.AppointmentId);

                // Check if the appointment exists
                var appointment = appointments.FirstOrDefault(a => a.Id == changeAppointmentDTO.AppointmentId);
                if (appointment == null)
                {
                    return new ResponseDTO { Status = 400, Message = "Failed to update appointment. Appointment not found." };
                }

                // Update appointment details
                appointment.ScheduleStartTime = changeAppointmentDTO.NewAppointmentStartTime;
                appointment.ConsultingDoctor = changeAppointmentDTO.NewConsultingDoctor;

                // Call the repository method to change the appointment
                bool appointmentChanged = await _receptionistRepo.ChangeAppointment(changeAppointmentDTO.AppointmentId, changeAppointmentDTO.NewAppointmentStartTime, changeAppointmentDTO.NewConsultingDoctor);

                if (appointmentChanged)
                {
                    // Send email with updated appointment details
                    string fullName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}";
                    string emailBody = $"Dear {fullName},<br><br>Your appointment has been updated successfully.<br><br>New Appointment Date: {appointment.ScheduleStartTime}<br>New Consulting Doctor: {appointment.ConsultingDoctor}<br><br>Regards,<br>Team";

                    // Create EmailDTO object
                    var emailDTO = new EmailDTO
                    {
                        Email = appointment.Patient.Email,
                        Subject = "Appointment Updated Successfully",
                        Body = emailBody
                    };

                    // Send email asynchronously
                    _emailService.SendEmailAsync(emailDTO);

                    return new ResponseDTO { Status = 200, Message = "Appointment updated successfully. Email sent to patient." };
                }
                else
                {
                    return new ResponseDTO { Status = 400, Message = "Failed to update appointment." };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Message = "An error occurred while updating appointment.", Error = ex.Message };
            }
        }

        #endregion


        //Addiional Methods

        #region Password Hash
        //Password Hash
        private string CreatePasswordHash(string Password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password)); //string to hash  
                string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant(); //hash to string
                return hashString;
            }
        }
        #endregion
    }
}
