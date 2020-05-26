using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using Serilog;

namespace DetailingArsenal.Application {
    /// <summary>
    /// When a new user is generated, go ahead and email Ed.
    /// </summary>
    public class NotifyEdOfNewUser : IBusEventHandler<NewUserEvent> {
        private IEmailClient emailClient;

        public NotifyEdOfNewUser(IEmailClient emailClient) {
            this.emailClient = emailClient;
        }

        public async Task Handle(NewUserEvent busEvent) {
            try {
                MailMessage message = new MailMessage();
                message.To.Add("me@eddieabbondanz.io");
                message.Subject = "New user signed up!";
                message.Body = $"A new user signed up! {busEvent.User.Email}";
                message.IsBodyHtml = false;
                message.BodyEncoding = UTF8Encoding.UTF8;

                await emailClient.Send(message);
            } catch (Exception e) {
                Log.Fatal(e, "Something went wrong sending an email to Ed.");
            }
        }
    }
}