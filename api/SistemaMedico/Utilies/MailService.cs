using System.Net;
using System.Net.Mail;

namespace SistemaMedico.Utilies
{
    public class EmailService
    {
        private readonly string _email;
        private readonly string _password;
        private readonly string _host;
        private readonly int _port;
        private readonly bool _enableSsl;
        private readonly string _from;

        public EmailService(IConfiguration configuration)
        {
            var emailSection = configuration.GetSection("Email");
            _email = emailSection["Username"] ?? string.Empty;
            _password = emailSection["Password"] ?? string.Empty;
            _host = emailSection["Host"] ?? "smtp.gmail.com";
            _port = int.TryParse(emailSection["Port"], out var parsedPort) ? parsedPort : 587;
            _enableSsl = bool.TryParse(emailSection["EnableSsl"], out var parsedSsl) ? parsedSsl : true;
            _from = emailSection["From"] ?? _email;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_host)
            {
                Port = _port,
                Credentials = new NetworkCredential(_email, _password),
                EnableSsl = _enableSsl,
            };

            var message = new MailMessage(_from, toEmail, subject, body)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(message);
        }

        public async Task SendPaymentLinkAsync(string toEmail, string paymentLink)
        {
            string subject = "Link de Pagamento do seu Tratamento";
            string body = $"Olá, segue o link para realizar o pagamento: {paymentLink}";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
