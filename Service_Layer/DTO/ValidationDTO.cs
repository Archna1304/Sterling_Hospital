namespace Service_Layer.DTO
{
    public class ValidationErrorDTO
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
