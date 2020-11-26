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

namespace DetailingArsenal.Application.Common {
    public class UpdateRolesOnInvoiceUpdated : IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated> {
        ICustomerRepo customerRepo;
        IRoleService roleService;
        IUserRepo userRepo;
        ISubscriptionPlanRepo subscriptionPlanRepo;

        public UpdateRolesOnInvoiceUpdated(ICustomerRepo customerRepo, IRoleService roleService, IUserRepo userRepo, ISubscriptionPlanRepo planRepo) {
            this.customerRepo = customerRepo;
            this.roleService = roleService;
            this.userRepo = userRepo;
            this.subscriptionPlanRepo = planRepo;
        }

        public async Task Notify(CustomerSubscriptionInvoiceUpdated busEvent) {
            var customer = await customerRepo.FindByBillingId(busEvent.CustomerBillingId) ?? throw new EntityNotFoundException();
            var user = await userRepo.FindById(customer.UserId) ?? throw new EntityNotFoundException();
            var plan = await subscriptionPlanRepo.FindById(busEvent.PlanId) ?? throw new EntityNotFoundException();

            var userRoles = await roleService.GetByUser(user);
            var planRole = await roleService.TryGetByName(plan.Name) ?? throw new EntityNotFoundException();
            var expiredRole = await roleService.TryGetByName("Expired") ?? throw new EntityNotFoundException();

            switch (busEvent.SubscriptionStatus) {
                case SubscriptionStatus.Active:
                case SubscriptionStatus.Trialing:
                case SubscriptionStatus.Incomplete:
                case SubscriptionStatus.Unpaid:
                    await roleService.AddRoleToUser(planRole, user, true);
                    break;
                case SubscriptionStatus.IncompleteExpired:
                case SubscriptionStatus.PastDue:
                case SubscriptionStatus.Canceled:
                    await roleService.AddRoleToUser(expiredRole, user, true);
                    break;
            }
        }
    }
}