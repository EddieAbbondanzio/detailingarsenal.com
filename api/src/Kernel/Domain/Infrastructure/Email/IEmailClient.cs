using System.Net.Mail;
using System.Threading.Tasks;

public interface IEmailClient {
    Task Send(MailMessage message);
}