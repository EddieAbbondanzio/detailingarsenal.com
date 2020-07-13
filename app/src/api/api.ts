import { AppointmentService } from '@/api/calendar/services/appointment-service';
import { PermissionService } from '@/api/security/services/permission-service';
import { RoleService } from '@/api/security/services/role-service';
import { SubscriptionPlanService } from '@/api/billing/services/subscription-plan-service';
import { AuthenticationService } from '@/api/users/services/authentication-service';
import { UserService } from '@/api/users/services/user-service';
import { ClientService } from '@/api/clients/services/client-service';
import { BusinessService } from '@/api/settings/services/business-service';
import { HoursOfOperationService } from '@/api/settings/services/hours-of-operation-service';
import { ServiceService } from '@/api/settings/services/service-service';
import { VehicleCategoryService } from '@/api/settings/services/vehicle-category-service';
import { CustomerService } from '@/api/billing/services/customer-service';
import { CheckoutSessionService } from '@/api/billing/services/checkout-session-service';

export const api = {
    authentication: new AuthenticationService(),
    user: new UserService(),
    appointment: new AppointmentService(),
    client: new ClientService(),
    /**
     * User defined settings section.
     */
    settings: {
        business: new BusinessService(),
        hoursOfOperation: new HoursOfOperationService(),
        service: new ServiceService(),
        vehicleCategory: new VehicleCategoryService()
    },
    /**
     * Role based access control section for all things permssions.
     */
    security: {
        permission: new PermissionService(),
        role: new RoleService()
    },
    /**
     * Billing section for everything related to subscriptions
     */
    billing: {
        subscriptionPlan: new SubscriptionPlanService(),
        customer: new CustomerService(),
        checkoutSession: new CheckoutSessionService()
    }
};
