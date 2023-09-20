using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;


namespace Dashboard.Utillities.Helper.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void SendEmail(string email, string content, string title)
        {
            // Reading Values from AppSetting JSON
            var from = configuration.GetSection("EmailServer:From").Value;
            var server = configuration.GetSection("EmailServer:SmtpServer").Value;
            var user = configuration.GetSection("EmailServer:user").Value;
            var Password = configuration.GetSection("EmailServer:Password").Value;
            var port = Convert.ToInt32(configuration.GetSection("EmailServer:Port").Value);


            // creatinng Email message 
            var Email = new MimeMessage();
            Email.From.Add(MailboxAddress.Parse(from));
            Email.To.Add(MailboxAddress.Parse(email));
            Email.Subject = title;
            Email.Body = new TextPart(TextFormat.Html) { Text = content };


            // Preparing Email for sending and then  Sending it out
            using var smtp = new SmtpClient();
            smtp.Connect(server, port, SecureSocketOptions.StartTls);
            smtp.Authenticate(user, Password);
            smtp.Send(Email);
            //smtp.Dispose();
            smtp.Disconnect(true);
        }
    }
}
