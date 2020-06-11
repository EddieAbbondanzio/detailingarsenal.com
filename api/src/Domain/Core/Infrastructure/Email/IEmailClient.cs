using System.Net.Mail;
using System.Threading.Tasks;

namespace DetailingArsenal.Domain {
    public interface IEmailClient {
        Task Send(MailMessage message);
    }
}