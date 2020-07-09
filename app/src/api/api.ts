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
import { SubscriptionService } from '@/api/billing/services/subscription-service';

export const api = {
    authentication: new AuthenticationService(),
    user: new UserService(),
    appointment: new AppointmentService(),
    client: new ClientService(),
    settings: {
        business: new BusinessService(),
        hoursOfOperation: new HoursOfOperationService(),
        service: new ServiceService(),
        vehicleCategory: new VehicleCategoryService()
    },
    security: {
        permission: new PermissionService(),
        role: new RoleService()
    },
    billing: {
        subscriptionPlan: new SubscriptionPlanService(),
        subscription: new SubscriptionService()
    }
};
