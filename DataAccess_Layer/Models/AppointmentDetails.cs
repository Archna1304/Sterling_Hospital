using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess_Layer.Models
{

    #region enum
    public enum Status
    {
        Scheduled = 1,
        Cancelled = 2,
        Rescheduled = 3
    }
    #endregion 

    public class AppointmentDetails
    {
        [Key]
        public int Id { get; set; }
  
        [Required]
        [ForeignKey("User")]
        public int PatientId { get; set; } // Foreign key referencing User table
        [JsonIgnore]
        public virtual User Patient { get; set; }

        [Required]
        public DateTime ScheduleStartTime { get; set; }

        [Required]
        public DateTime ScheduleEndTime { get; set; }

        [Required]
        public string PatientProblem { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public string ConsultingDoctor { get; set; }

        [ForeignKey("User")]
        public int? NurseId { get; set; }

        [JsonIgnore]
        public virtual User Nurse { get; set; }

    }
}
    
