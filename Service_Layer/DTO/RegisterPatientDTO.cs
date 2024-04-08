using DataAccess_Layer.Models;
using System.ComponentModel.DataAnnotations;

namespace Service_Layer.DTO
{
    public class RegisterPatientDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string? PostalCode { get; set; }
        public string Role { get; set; }
        public string Password { get;}
    }
}
