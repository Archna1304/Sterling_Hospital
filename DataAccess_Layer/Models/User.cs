using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess_Layer.Models
{

    #region enum
    public enum Role
    {
        Doctor = 1,
        Nurse = 2,
        Receptionist = 3,
        Patient = 4
    }

    public enum Sex
    {
        Male = 1,
        Female = 2,
        Other = 3
    }
    #endregion

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [Column (TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Sex Sex { get; set; }

        [Required]
        public string Address { get; set; }
        
        public string? PostalCode { get; set; }

        [JsonIgnore]
        [Column("Password")]
        public string Password { get; set; }
        
        public Role Role { get; set; }

        
    }
}
