using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Application.Common {
    public class EmailEdOnNewUser : IDomainEventSubscriber<NewUserCreatedEvent> {
        IEmailClient emailClient;

        public EmailEdOnNewUser(IEmailClient emailClient) {
            this.emailClient = emailClient;
        }

        public async Task Notify(NewUserCreatedEvent busEvent) {
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