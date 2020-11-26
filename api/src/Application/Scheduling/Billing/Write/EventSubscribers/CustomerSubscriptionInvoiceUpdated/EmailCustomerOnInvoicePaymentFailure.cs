using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Scheduling.Billing {
    public class EmailCustomerOnInvoicePaymentFailure : IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated> {
        IEmailClient emailClient;
        ICustomerRepo customerRepo;
        IUserRepo userRepo;

        public EmailCustomerOnInvoicePaymentFailure(IEmailClient emailClient, ICustomerRepo customerRepo, IUserRepo userRepo) {
            this.emailClient = emailClient;
            this.customerRepo = customerRepo;
            this.userRepo = userRepo;
        }

        public async Task Notify(CustomerSubscriptionInvoiceUpdated busEvent) {
            var customer = await customerRepo.FindByBillingId(busEvent.CustomerBillingId) ?? throw new EntityNotFoundException();
            var user = await userRepo.FindById(customer.UserId) ?? throw new EntityNotFoundException();

            MailMessage message = new MailMessage();
            message.To.Add(user.Email);
            message.Subject = "An error occured processing your subscription payment";
            message.Body = @$"Hello! Just wanted to reach out and let you know there was an issue processing your payment for your Detailing Arsenal subscription. If you enjoy using our service, and would like to continue using it, please update the payment method on file for your account. Thanks!";
            message.IsBodyHtml = false;
            message.BodyEncoding = UTF8Encoding.UTF8;

            await emailClient.Send(message);
        }
    }
}