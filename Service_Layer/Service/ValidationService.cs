using Service_Layer.DTO;
using Service_Layer.Services;
using System.Text.RegularExpressions;

namespace RepoPatternSports.Service.Service
{
    public class ValidationService : IValidationService
    {
        #region Login DTO

        public async Task<ValidationErrorDTO> ValidateLogin(LoginDTO loginDTO)
        {
            var response = new ValidationErrorDTO();
            response.Status = 200;
            response.Message = "Validation successful.";
            response.Errors = new List<string>();

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(loginDTO.Email) && string.IsNullOrWhiteSpace(loginDTO.PhoneNumber))
            {
                errors.Add("Email or phone number is required.");
            }

            if (string.IsNullOrWhiteSpace(loginDTO.Password))
            {
                errors.Add("Password is required.");
            }

            response.Status = errors.Any() ? 400 : 200;
            response.Message = errors.Any() ? "Validation failed." : "Validation successful.";
            response.Errors = errors;

            return await Task.FromResult(response);
        }

        #endregion

        #region Register DTO validation
        public async Task<ValidationErrorDTO> ValidateUser(RegisterDTO registerDTO)
        {
            ValidationErrorDTO response = new ValidationErrorDTO();
            response.Status = 200;
            response.Message = "Validation successful.";
            response.Errors = new List<string>();


            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(registerDTO.FirstName))
            {
                errors.Add("Firstname is required.");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.LastName))
            {
                errors.Add("Lastname is required.");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Email))
            {
                errors.Add("Email is required.");
            }
            else if (!IsValidEmail(registerDTO.Email))
            {
                errors.Add("Invalid email format.");
            }

            if (!string.IsNullOrWhiteSpace(registerDTO.PhoneNumber) && !IsValidPhoneNumber(registerDTO.PhoneNumber))
            {
                errors.Add("Invalid contact number format.");
            }

            if (registerDTO.DateOfBirth == default(DateTime))
            {
                response.Errors.Add("Date of birth is required.");
            }
            else if (registerDTO.DateOfBirth.Date == DateTime.Today)
            {
                response.Errors.Add("Date of birth cannot be the current date.");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Sex))
            {
                response.Errors.Add("Gender is required.");
            }
            else
            {
                string[] validGenders = { "male", "female", "other" };
                if (!validGenders.Contains(registerDTO.Sex.ToLower()))
                {
                    response.Errors.Add("Invalid gender. Gender must be 'male', 'female', or 'other'.");
                }
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Address))
            {
                response.Errors.Add("Address is required.");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.PostalCode))
            {
                response.Errors.Add("Postal code is required.");
            }
            else if (!IsValidPostalCode(registerDTO.PostalCode))
            {
                response.Errors.Add("Invalid postal code format.");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Password))
            {
                response.Errors.Add("Password is required.");
            }
            else if (!IsValidPassword(registerDTO.Password))
            {
                response.Errors.Add("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.");
            }

            if (string.IsNullOrWhiteSpace(registerDTO.Role))
            {
                response.Errors.Add("Role is required.");
            }

            if (errors.Any())
            {
                response.Status = 400;
                response.Message = "Validation failed.";
                response.Errors = errors;
            }

            return await Task.FromResult(response);
        }
        #endregion

        #region Register doctor DTO

        public async Task<ValidationErrorDTO> ValidateDoctor(RegisterDoctorDTO registerDoctorDTO)
        {
            var response = new ValidationErrorDTO();
            response.Status = 200;
            response.Message = "Validation successful.";
            response.Errors = new List<string>();

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(registerDoctorDTO.FirstName))
            {
                errors.Add("Firstname is required.");
            }

            if (string.IsNullOrWhiteSpace(registerDoctorDTO.LastName))
            {
                errors.Add("Lastname is required.");
            }

            if (string.IsNullOrWhiteSpace(registerDoctorDTO.Email))
            {
                errors.Add("Email is required.");
            }
            else if (!IsValidEmail(registerDoctorDTO.Email))
            {
                errors.Add("Invalid email format.");
            }

            if (!string.IsNullOrWhiteSpace(registerDoctorDTO.PhoneNumber) && !IsValidPhoneNumber(registerDoctorDTO.PhoneNumber))
            {
                errors.Add("Invalid contact number format.");
            }

            if (registerDoctorDTO.DateOfBirth == default(DateTime))
            {
                errors.Add("Date of birth is required.");
            }
            else if (registerDoctorDTO.DateOfBirth == DateTime.Today)
            {
                errors.Add("Date of birth cannot be the current date.");
            }

            if (string.IsNullOrWhiteSpace(registerDoctorDTO.Sex))
            {
                errors.Add("Gender is required.");
            }
            else
            {
                string[] validGenders = { "male", "female", "other" };
                if (!validGenders.Contains(registerDoctorDTO.Sex.ToLower()))
                {
                    errors.Add("Invalid gender. Gender must be 'male', 'female', or 'other'.");
                }
            }

            if (string.IsNullOrWhiteSpace(registerDoctorDTO.Address))
            {
                errors.Add("Address is required.");
            }

            if (!string.IsNullOrWhiteSpace(registerDoctorDTO.PostalCode) && !IsValidPostalCode(registerDoctorDTO.PostalCode))
            {
                errors.Add("Invalid postal code format.");
            }

            if (string.IsNullOrWhiteSpace(registerDoctorDTO.Password))
            {
                errors.Add("Password is required.");
            }
            else if (!IsValidPassword(registerDoctorDTO.Password))
            {
                errors.Add("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.");
            }

            if (string.IsNullOrWhiteSpace(registerDoctorDTO.Role))
            {
                errors.Add("Role is required.");
            }

            if (string.IsNullOrWhiteSpace(registerDoctorDTO.Specialization))
            {
                errors.Add("Specialization is required.");
            }
            else
            {
                string[] validSpecializations = { "BrainSurgery", "Physiotherapist", "EyeSpecialist" };
                if (!validSpecializations.Contains(registerDoctorDTO.Specialization))
                {
                    errors.Add("Invalid specialization. Specialization must be 'BrainSurgery', 'Physiotherapist', or 'EyeSpecialist'.");
                }
            }

            response.Status = errors.Any() ? 400 : 200;
            response.Message = errors.Any() ? "Validation failed." : "Validation successful.";
            response.Errors = errors;

            return await Task.FromResult(response);
        }

        #endregion

        #region change Appointment DTo

        public async Task<ValidationErrorDTO> ValidateChangeAppointment(ChangeAppointmentDTO changeAppointmentDTO)
        {
            var response = new ValidationErrorDTO();
            response.Status = 200;
            response.Message = "Validation successful.";
            response.Errors = new List<string>();

            var errors = new List<string>();

            if (changeAppointmentDTO.AppointmentId <= 0)
            {
                errors.Add("AppointmentId must be greater than 0.");
            }

            if (changeAppointmentDTO.NewAppointmentStartTime == default(DateTime))
            {
                errors.Add("NewAppointmentStartTime is required.");
            }

            if (changeAppointmentDTO.NewAppointmentEndTime == default(DateTime))
            {
                errors.Add("NewAppointmentEndTime is required.");
            }
            else if (changeAppointmentDTO.NewAppointmentEndTime <= changeAppointmentDTO.NewAppointmentStartTime)
            {
                errors.Add("NewAppointmentEndTime must be after NewAppointmentStartTime.");
            }

            if (string.IsNullOrWhiteSpace(changeAppointmentDTO.NewConsultingDoctor))
            {
                errors.Add("NewConsultingDoctor is required.");
            }
            else
            {
                string[] validDoctors = { "BrainSurgery", "Physiotherapist", "EyeSpecialist" };
                if (!validDoctors.Contains(changeAppointmentDTO.NewConsultingDoctor))
                {
                    errors.Add("Invalid consulting doctor. Must be 'BrainSurgery', 'Physiotherapist', or 'EyeSpecialist'.");
                }
            }

            response.Status = errors.Any() ? 400 : 200;
            response.Message = errors.Any() ? "Validation failed." : "Validation successful.";
            response.Errors = errors;

            return await Task.FromResult(response);
        }

        #endregion

        #region Assign Nurse DTO

        public async Task<ValidationErrorDTO> ValidateAssignNurse(AssignNurseDTO assignNurseDTO)
        {
            var response = new ValidationErrorDTO();
            response.Status = 200;
            response.Message = "Validation successful.";
            response.Errors = new List<string>();

            var errors = new List<string>();

            if (assignNurseDTO.AppointmentId <= 0)
            {
                errors.Add("AppointmentId must be greater than 0.");
            }

            if (assignNurseDTO.NurseId <= 0)
            {
                errors.Add("NurseId must be greater than 0.");
            }

            response.Status = errors.Any() ? 400 : 200;
            response.Message = errors.Any() ? "Validation failed." : "Validation successful.";
            response.Errors = errors;

            return await Task.FromResult(response);
        }


        #endregion

        #region Email DTO

        public async Task<ValidationErrorDTO> ValidateEmail(EmailDTO emailDTO)
        {
            var response = new ValidationErrorDTO();
            response.Status = 200;
            response.Message = "Validation successful.";
            response.Errors = new List<string>();

            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(emailDTO.Email))
            {
                errors.Add("Email is required.");
            }
            else if (!IsValidEmail(emailDTO.Email))
            {
                errors.Add("Invalid email format.");
            }

            if (string.IsNullOrWhiteSpace(emailDTO.Subject))
            {
                errors.Add("Subject is required.");
            }

            if (string.IsNullOrWhiteSpace(emailDTO.Body))
            {
                errors.Add("Body is required.");
            }

            response.Status = errors.Any() ? 400 : 200;
            response.Message = errors.Any() ? "Validation failed." : "Validation successful.";
            response.Errors = errors;

            return await Task.FromResult(response);
        }

        #endregion

        #region register patient Dto
        public async Task<ValidationErrorDTO> ValidatePatient(RegisterPatientDTO registerPatientDTO)
        {
            ValidationErrorDTO response = new ValidationErrorDTO();
            response.Status = 200;
            response.Message = "Validation successful.";
            response.Errors = new List<string>();

            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(registerPatientDTO.FirstName))
            {
                errors.Add("Firstname is required.");
            }

            if (string.IsNullOrWhiteSpace(registerPatientDTO.LastName))
            {
                errors.Add("Lastname is required.");
            }

            if (string.IsNullOrWhiteSpace(registerPatientDTO.Email))
            {
                errors.Add("Email is required.");
            }
            else if (!IsValidEmail(registerPatientDTO.Email))
            {
                errors.Add("Invalid email format.");
            }

            if (string.IsNullOrWhiteSpace(registerPatientDTO.Sex))
            {
                errors.Add("Gender is required.");
            }
            else
            {
                string[] validGenders = { "male", "female", "other" };
                if (!validGenders.Contains(registerPatientDTO.Sex.ToLower()))
                {
                    errors.Add("Invalid gender. Gender must be 'male', 'female', or 'other'.");
                }
            }

            if (registerPatientDTO.DateOfBirth == default(DateTime))
            {
                errors.Add("Date of birth is required.");
            }
            else if (registerPatientDTO.DateOfBirth == DateTime.Today)
            {
                errors.Add("Date of birth cannot be the current date.");
            }

            if (!string.IsNullOrWhiteSpace(registerPatientDTO.PostalCode) && !IsValidPostalCode(registerPatientDTO.PostalCode))
            {
                errors.Add("Invalid postal code format.");
            }

            if (string.IsNullOrWhiteSpace(registerPatientDTO.Role))
            {
                errors.Add("Role is required.");
            }

            response.Status = errors.Any() ? 400 : 200;
            response.Message = errors.Any() ? "Validation failed." : "Validation successful.";
            response.Errors = errors;

            return await Task.FromResult(response);
        }
        #endregion

        #region Appointment DTO

        public async Task<ValidationErrorDTO> ValidateAppointmentDTO(AppointmentDTO appointmentDTO)
        {
            var response = new ValidationErrorDTO();
            response.Status = 200;
            response.Message = "Validation successful.";
            response.Errors = new List<string>();

            var errors = new List<string>();

            if (appointmentDTO.PatientId <= 0)
            {
                errors.Add("PatientId must be greater than 0.");
            }

            if (appointmentDTO.ScheduleStartTime == default(DateTime))
            {
                errors.Add("ScheduleStartTime is required.");
            }

            if (appointmentDTO.ScheduleEndTime == default(DateTime))
            {
                errors.Add("ScheduleEndTime is required.");
            }

            if (appointmentDTO.ScheduleStartTime >= appointmentDTO.ScheduleEndTime)
            {
                errors.Add("ScheduleStartTime must be before ScheduleEndTime.");
            }

            if (string.IsNullOrWhiteSpace(appointmentDTO.PatientProblem))
            {
                errors.Add("PatientProblem is required.");
            }

            if (string.IsNullOrWhiteSpace(appointmentDTO.Description))
            {
                errors.Add("Description is required.");
            }

            var validStatuses = new List<string> { "scheduled", "cancelled", "rescheduled" };
            if (string.IsNullOrWhiteSpace(appointmentDTO.Status) || !validStatuses.Contains(appointmentDTO.Status.ToLower()))
            {
                errors.Add("Invalid status. Status must be 'scheduled', 'cancelled', or 'rescheduled'.");
            }

            if (string.IsNullOrWhiteSpace(appointmentDTO.ConsultingDoctor))
            {
                errors.Add("ConsultingDoctor is required.");
            }

            if (appointmentDTO.DoctorId <= 0)
            {
                errors.Add("DoctorId must be greater than 0.");
            }

            response.Status = errors.Any() ? 400 : 200;
            response.Message = errors.Any() ? "Validation failed." : "Validation successful.";
            response.Errors = errors;

            return await Task.FromResult(response);
        }

        #endregion

        #region IsValid Email, phoneNumber, postal code, password
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return Regex.Match(phoneNumber, @"^\d{10}$").Success;
        }

        public static bool IsValidPostalCode(string postalCode)
        {
            return Regex.Match(postalCode, @"^\d{5}$").Success;
        }

        public static bool IsValidPassword(string password)
        {
            return Regex.Match(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$").Success;
        }

        #endregion
    }



}

