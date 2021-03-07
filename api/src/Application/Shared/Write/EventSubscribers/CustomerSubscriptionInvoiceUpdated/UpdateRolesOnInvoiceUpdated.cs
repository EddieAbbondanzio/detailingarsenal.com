using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Scheduling.Billing;
using DetailingArsenal.Domain.Users.Security;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Application {
    [DependencyInjection(RegisterAs = typeof(IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated>))]
    public class UpdateRolesOnInvoiceUpdated : IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated> {
        ICustomerRepo customerRepo;
        IRoleAssigner roleAssigner;
        IUserRepo userRepo;
        ISubscriptionPlanRepo subscriptionPlanRepo;

        public UpdateRolesOnInvoiceUpdated(ICustomerRepo customerRepo, IRoleAssigner roleService, IUserRepo userRepo, ISubscriptionPlanRepo planRepo) {
            this.customerRepo = customerRepo;
            this.roleAssigner = roleService;
            this.userRepo = userRepo;
            this.subscriptionPlanRepo = planRepo;
        }

        public async Task Notify(CustomerSubscriptionInvoiceUpdated busEvent) {
            var customer = await customerRepo.FindByBillingId(busEvent.CustomerBillingId) ?? throw new EntityNotFoundException();
            var user = await userRepo.FindById(customer.UserId) ?? throw new EntityNotFoundException();


            switch (busEvent.SubscriptionStatus) {
                case SubscriptionStatus.Active:
                case SubscriptionStatus.Trialing:
                case SubscriptionStatus.Incomplete:
                case SubscriptionStatus.Unpaid:
                    var plan = await subscriptionPlanRepo.FindById(busEvent.PlanId) ?? throw new EntityNotFoundException();
                    await roleAssigner.ReplaceRoles(user, (Guid)plan.RoleId!); //TODO: Fix this
                    break;
                case SubscriptionStatus.IncompleteExpired:
                case SubscriptionStatus.PastDue:
                case SubscriptionStatus.Canceled:
                    await roleAssigner.ReplaceRoles(user, "Free");
                    break;
            }
        }
    }
}