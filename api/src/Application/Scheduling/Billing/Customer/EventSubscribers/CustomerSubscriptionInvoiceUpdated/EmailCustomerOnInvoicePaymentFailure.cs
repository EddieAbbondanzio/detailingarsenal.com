using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Application.Billing {
    public class EmailCustomerOnInvoicePaymentFailure : IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated> {
        IEmailClient emailClient;
        ICustomerService customerService;
        IUserService userService;

        public EmailCustomerOnInvoicePaymentFailure(IEmailClient emailClient, ICustomerService customerService, IUserService userService) {
            this.emailClient = emailClient;
            this.customerService = customerService;
            this.userService = userService;
        }

        public async Task Notify(CustomerSubscriptionInvoiceUpdated busEvent) {
            var customer = await customerService.GetByBillingId(busEvent.CustomerBillingId);
            var user = await userService.GetUserById(customer.UserId);

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