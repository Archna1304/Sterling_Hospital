namespace Service_Layer.DTO
{
    public class DoctorSpecializationDTO
    {
      
        public int UserId { get; set; } // Foreign key referencing User table
       

        public string Specialization { get; set; }
    }
}
