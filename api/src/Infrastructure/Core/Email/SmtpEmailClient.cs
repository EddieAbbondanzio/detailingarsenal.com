using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DetailingArsenal.Domain;

namespace DetailingArsenal.Infrastructure {
    public class SmtpEmailClient : IEmailClient {
        private SmtpClient client;
        private MailAddress from;

        public SmtpEmailClient(EmailConfig config) {
            client = new SmtpClient(config.SMTP, config.Port);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(config.Username, config.Password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;

            from = new MailAddress(config.Username);
        }

        public async Task Send(MailMessage message) {
            message.From = from;
            await client.SendMailAsync(message);
        }
    }
}