namespace Service_Layer.DTO
{
    public class ChangeAppointmentDTO
    {
        public int AppointmentId { get; set; }
        public DateTime NewAppointmentStartTime { get; set; }
        public string NewConsultingDoctor { get; set; }
    }
}
