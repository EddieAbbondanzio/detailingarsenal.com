using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class SmtpEmailClient : IEmailClient {
    private SmtpClient client;

    public SmtpEmailClient(EmailConfig config) {
        client = new SmtpClient(config.SMTP);
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential(config.Username, config.Password);
    }

    public async Task Send(MailMessage message) {
        await client.SendMailAsync(message);
    }
}