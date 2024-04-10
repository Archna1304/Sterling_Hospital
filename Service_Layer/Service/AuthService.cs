using AutoMapper;
using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service_Layer.DTO;
using Service_Layer.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Service_Layer.Service
{
    public class AuthService : IAuthService
    {
        #region prop
        private readonly IAuthRepo _authRepo;
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IDoctorRepo _doctorRepo;

        #endregion

        #region Constructor
        public AuthService(IAuthRepo authRepo, IConfiguration configuration, IEmailService emailService, IUserRepo userRepo, IMapper mapper, IDoctorRepo doctorRepo)
        {
            _authRepo = authRepo;
            _configuration = configuration;
            _emailService = emailService;
            _userRepo = userRepo;
            _mapper = mapper;
            _doctorRepo = doctorRepo;
        }
        #endregion

        // Method

        #region Register Method
        public async Task<ResponseDTO> Register(RegisterDTO registerDTO)
        {
            try
            {
                // Map RegisterDTO to User entity
                var user = _mapper.Map<User>(registerDTO);

                string passwordHash;
                // Password For other roles (doctor, receptionist, nurse)
                passwordHash = CreatePasswordHash(registerDTO.Password);

                // Check if the role is patient, and disallow registration
                if (registerDTO.Role.ToLower() == "patient")
                {
                    return new ResponseDTO { Status = 400, Message = "Patients cannot be registered by this Method." };
                }

                // Check if the role is patient, and disallow registration
                if (registerDTO.Role.ToLower() == "doctor")
                {
                    return new ResponseDTO { Status = 400, Message = "Doctors cannot be registered by this Method." };
                }

                // Check if the role limit is exceeded
                bool roleLimitExceeded = await CheckRoleLimit(user.Role);
                if (roleLimitExceeded)
                {
                    string roleMessage = $"Role limit exceeded. There can be only {_roleLimits[user.Role]} {user.Role.ToString().ToLower()}s in the system.";
                    return new ResponseDTO { Status = 400, Message = roleMessage };
                }

                user.Password = passwordHash; // Set the hashed password

                // Register user
                bool registered = await _authRepo.Register(user);

                if (registered)
                {
                    string FullName = $"{registerDTO.FirstName} {registerDTO.LastName}";
                    // Send email to user
                    var emailDTO = new EmailDTO
                    {
                        Email = user.Email,
                        Subject = "Registration Successful",
                        Body = $"Dear {FullName},<br><br> Welcome to Sterling Hospital.<br><br> Congratulations!! Your registration was successful as a Role: {registerDTO.Role}.<br><br>Your password is: {registerDTO.Password}<br><br>Regards,<br>Sterling Hospital"
                    };

                    _emailService.SendEmailAsync(emailDTO);

                    return new ResponseDTO { Status = 200, Message = "Registration successful. Email sent to user." };
                }
                else
                {
                    return new ResponseDTO { Status = 500, Message = "Failed to register user." };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Message = "An error occurred while registering user.", Error = ex.Message };
            }
        }

        #endregion

        #region Register Doctor Method
        public async Task<ResponseDTO> RegisterDoctor(RegisterDoctorDTO registerDoctorDTO)
        {
            try
            {
                // Map RegisterDoctorDTO to User entity
                var user = _mapper.Map<User>(registerDoctorDTO);

                // Set the hashed password
                string passwordHash = CreatePasswordHash(registerDoctorDTO.Password);
                user.Password = passwordHash;

                // Map specialization string to enum
                var specialization = _mapper.Map<Specialization>(registerDoctorDTO.Specialization);

                // Check if the role is doctor and the doctor limit is exceeded
                if (user.Role == Role.Doctor)
                {
                    bool roleLimitExceeded = await CheckRoleLimit(Role.Doctor, specialization);
                    if (roleLimitExceeded)
                    {
                        string roleMessage = $"Role limit exceeded. There can be only {_roleLimits[Role.Doctor]} doctors in the system.";
                        return new ResponseDTO { Status = 400, Message = roleMessage };
                    }
                }


                // Register user
                bool registered = await _authRepo.Register(user);

               

                if (registered)
                {
                    // Map specialization string to enum
                    var doctorSpecialization = _mapper.Map<Specialization>(registerDoctorDTO.Specialization);

                    // Update DoctorSpecialization
                    bool specializationUpdated = await _doctorRepo.UpdateDoctorSpecialization(specialization, user.UserId);

                    if (specializationUpdated)
                    {
                        // Send email to user
                        var emailDTO = new EmailDTO
                        {
                            Email = user.Email,
                            Subject = "Registration Successful",
                            Body = $"Dear {user.FirstName} {user.LastName},<br><br> Welcome to Sterling Hospital.<br><br> Congratulations!! Your registration was successful as a {registerDoctorDTO.Role} specialized in {registerDoctorDTO.Specialization}.<br><br>Your password is: {registerDoctorDTO.Password}<br><br>Regards,<br>Sterling Hospital"
                        };

                        _emailService.SendEmailAsync(emailDTO);

                        return new ResponseDTO { Status = 200, Message = "Registration successful. Email sent to user." };
                    }
                    else
                    {
                        // Rollback registration if specialization update fails
                        await RollbackRegistration(user.UserId);
                        return new ResponseDTO { Status = 500, Message = "Failed to update doctor's specialization. Registration rolled back." };
                    }
                }
                else
                {
                    return new ResponseDTO { Status = 500, Message = "Failed to register user." };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Message = "An error occurred while registering user.", Error = ex.Message };
            }
        }


        #endregion

        #region Login Method

        public async Task<ResponseDTO> Login(LoginDTO loginDTO)
        {
            try
            {
                // Password Hash
                string passwordHash = CreatePasswordHash(loginDTO.Password);

                User loggedInUser = null;
                // Check if loginDTO contains email or phone number
                if (!string.IsNullOrEmpty(loginDTO.PhoneNumberOrEmail))
                {
                    // Login with email or phone number
                    loggedInUser = await _authRepo.LoginWithEmailOrPhoneNumber(loginDTO.PhoneNumberOrEmail, passwordHash);

                    if (loggedInUser == null)
                    {
                        // Neither email nor phone number provided
                        return new ResponseDTO { Status = 401, Message = "Invalid credentials" };
                    }
                }
                else
                {
                    // Neither email nor phone number provided
                    return new ResponseDTO { Status = 400, Message = "Email or phone number is required for login." };
                }

                if (loggedInUser != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, loggedInUser.Role.ToString()),
                    };

                    // Token Generation
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return new ResponseDTO { Status = 200, Message = "User logged in successfully", Data = jwtToken };
                }
                else
                {
                    return new ResponseDTO { Status = 401, Message = "Invalid credentials" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { Status = 500, Message = "Error occurred while logging in", Data = ex.Message };
            }
        }
        #endregion

        //Additional Methods

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

        #region RollBack method for doctor
        private async Task<bool> RollbackRegistration(int userId)
        {
            try
            {
                // Delete the user from the database
                var userToDelete = await _userRepo.GetUserById(userId);
                if (userToDelete != null)
                {
                    await _userRepo.DeleteUser(userToDelete);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Handle exception
                return false;
            }
        }
        #endregion

       
        //Role Limits 

        #region CheckRoleLimit
        private async Task<bool> CheckRoleLimit(Role role, Specialization? specialization = null)
        {
            try
            {
                switch (role)
                {
                    case Role.Doctor:
                        if (specialization != null)
                        {
                            int doctorCountBySpecialization = await _doctorRepo.CountDoctorsBySpecialization(specialization.Value);
                            return doctorCountBySpecialization >= 1; // Limit to 1 doctor per specialization
                        }
                        else
                        {
                            int doctorCount = await _userRepo.CountDoctor();
                            return doctorCount >= 3; // Limit to 3 doctors for unspecified specialization
                        }
                    case Role.Nurse:
                        int nurseCount = await _userRepo.CountNurse();
                        return nurseCount >= 10; // Limit to 10 nurses
                    case Role.Receptionist:
                        int receptionistCount = await _userRepo.CountReceptionist();
                        return receptionistCount >= 2; // Limit to 2 receptionists
                    default:
                        return false; // No limit for other roles
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine($"Error checking role limit: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Role Limits
        private Dictionary<Role, int> _roleLimits = new Dictionary<Role, int>
        {
            { Role.Nurse, 10 },
            { Role.Receptionist, 2 },
            { Role.Doctor,3 }

        };
        #endregion
    }
}
