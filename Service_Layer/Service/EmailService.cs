using Microsoft.Extensions.Configuration;
using Service_Layer.DTO;
using Service_Layer.Interface;
using System.Net;
using System.Net.Mail;

namespace Service_Layer.Service
{
    public class EmailService : IEmailService
    {
        #region prop
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        #endregion

        //Method

        #region Email Service
        public void SendEmailAsync(EmailDTO emailDTO)
        {
            string server = _configuration["EmailService:Server"];
            int port = int.Parse(_configuration["EmailService:Port"]);
            string emailFrom = _configuration["EmailService:EmailFrom"];
            string emailPass = _configuration["EmailService:EmailPass"];

            using (SmtpClient client = new SmtpClient(server, port))
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailFrom, emailPass);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(emailFrom);
                mailMessage.To.Add(emailDTO.Email);
                mailMessage.Subject = emailDTO.Subject;
                mailMessage.Body = emailDTO.Body;
                mailMessage.IsBodyHtml = true;

                client.Send(mailMessage);
            }

        }
        #endregion
    }
}
