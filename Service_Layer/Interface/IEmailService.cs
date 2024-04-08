using Service_Layer.DTO;

namespace Service_Layer.Interface
{
    public interface IEmailService
    {
        void SendEmailAsync(EmailDTO emailDTO);
    }
}
