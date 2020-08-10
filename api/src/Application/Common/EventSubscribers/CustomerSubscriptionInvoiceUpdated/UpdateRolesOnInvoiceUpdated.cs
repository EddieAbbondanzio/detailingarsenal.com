using System;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DetailingArsenal.Domain;
using DetailingArsenal.Domain.Billing;
using DetailingArsenal.Domain.Security;
using DetailingArsenal.Domain.Users;
using Serilog;

namespace DetailingArsenal.Application.Common {
    public class UpdateRolesOnInvoiceUpdated : IDomainEventSubscriber<CustomerSubscriptionInvoiceUpdated> {
        ICustomerService customerService;
        IRoleService roleService;
        IUserService userService;
        ISubscriptionPlanService planService;

        public UpdateRolesOnInvoiceUpdated(ICustomerService customerService, IRoleService roleService, IUserService userService, ISubscriptionPlanService planService) {
            this.customerService = customerService;
            this.roleService = roleService;
            this.userService = userService;
            this.planService = planService;
        }

        public async Task Notify(CustomerSubscriptionInvoiceUpdated busEvent) {
            var customer = await customerService.GetByBillingId(busEvent.CustomerBillingId);
            var user = await userService.GetUserById(customer.UserId);
            var plan = await planService.GetById(busEvent.PlanId);

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