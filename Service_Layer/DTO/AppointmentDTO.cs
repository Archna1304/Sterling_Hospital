namespace Service_Layer.DTO
{

    public class AppointmentDTO
    {
        public int PatientId { get; set; }
        public DateTime ScheduleStartTime { get; set; }
        public DateTime ScheduleEndTime { get; set; }
        public string PatientProblem { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string ConsultingDoctor { get; set; }
        public int? DoctorId { get; set; }


    }
}