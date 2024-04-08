using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess_Layer.Models
{

    #region enum
    public enum Specialization
    {
        BrainSurgery = 1,
        Physiotherapist = 2,
        EyeSpecialist = 3
    }
    #endregion

    public class DoctorSpecialization
    {
       
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("User")]

        public int UserId { get; set; } // Foreign key referencing User table
        [JsonIgnore]
        public virtual User User { get; set; }

        [Required]
        public Specialization Specialization { get; set; }
    }
}
