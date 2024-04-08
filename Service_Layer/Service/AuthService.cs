﻿using AutoMapper;
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
        #endregion

        #region Constructor
        public AuthService(IAuthRepo authRepo, IConfiguration configuration, IEmailService emailService, IUserRepo userRepo, IMapper mapper)
        {
            _authRepo = authRepo;
            _configuration = configuration;
            _emailService = emailService;
            _userRepo = userRepo;
            _mapper = mapper;
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

                // Check if the role limit is exceeded
                bool roleLimitExceeded = await CheckRoleLimit(user.Role);
                if (roleLimitExceeded)
                {
                    return new ResponseDTO { Status = 400, Message = "Role limit exceeded. Registration failed." };
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


        #region Login Method

        public async Task<ResponseDTO> Login(LoginDTO loginDTO)
        {
            try
            {
                // Password Hash
                string passwordHash = CreatePasswordHash(loginDTO.Password);

                User loggedInUser = null;

                // Check if loginDTO contains email or phone number
                if (!string.IsNullOrEmpty(loginDTO.Email))
                {
                    // Login with email
                    loggedInUser = await _authRepo.LoginWithEmail(loginDTO.Email, passwordHash);
                }
                else if (!string.IsNullOrEmpty(loginDTO.PhoneNumber))
                {
                    // Login with phone number
                    loggedInUser = await _authRepo.LoginWithPhoneNumber(loginDTO.PhoneNumber, passwordHash);
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

        #region CheckRoleLimit
        private async Task<bool> CheckRoleLimit(Role role)
        {
            try
            {
                switch (role)
                {
                    case Role.Doctor:
                        int doctorCount = await _userRepo.CountDoctor();
                        return doctorCount >= 3; // Limit to 3 doctors
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

    }
}
