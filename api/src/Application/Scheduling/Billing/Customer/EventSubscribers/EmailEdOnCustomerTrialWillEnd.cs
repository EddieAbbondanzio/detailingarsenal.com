using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Application.Billing {
    public class EmailEdOnCustomerTrialWillEnd : IDomainEventSubscriber<CustomerTrialWillEndSoon> {
        IEmailClient emailClient;
        ICustomerService customerService;
        IUserRepo userRepo;

        public EmailEdOnCustomerTrialWillEnd(IEmailClient emailClient, ICustomerService customerService, IUserRepo userRepo) {
            this.emailClient = emailClient;
            this.customerService = customerService;
            this.userRepo = userRepo;
        }

        public async Task Notify(CustomerTrialWillEndSoon busEvent) {
            var customer = await customerService.GetByBillingId(busEvent.CustomerBillingId);
            var user = await userRepo.FindById(customer.UserId) ?? throw new EntityNotFoundException();

            try {
                MailMessage message = new MailMessage();
                message.To.Add("me@eddieabbondanz.io");
                message.Subject = "User trial ending soon!";
                message.Body = $"User trial is going to end soon. Email them. {user.Email}";
                message.IsBodyHtml = false;
                message.BodyEncoding = UTF8Encoding.UTF8;

                await emailClient.Send(message);
            } catch (Exception e) {
                Log.Fatal(e, "Something went wrong sending an email to Ed.");
            }
        }
    }
}