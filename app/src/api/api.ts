import { AppointmentService } from '@/api/scheduling/calendar/services/appointment-service';
import { PermissionService } from '@/api/scheduling/security/services/permission-service';
import { RoleService } from '@/api/scheduling/security/services/role-service';
import { SubscriptionPlanService } from '@/api/scheduling/billing/services/subscription-plan-service';
import { AuthenticationService } from '@/api/user/services/authentication-service';
import { UserService } from '@/api/user/services/user-service';
import { ClientService } from '@/api/scheduling/clients/services/client-service';
import { BusinessService } from '@/api/scheduling/settings/services/business-service';
import { HoursOfOperationService } from '@/api/scheduling/settings/services/hours-of-operation-service';
import { ServiceService } from '@/api/scheduling/settings/services/service-service';
import { VehicleCategoryService } from '@/api/scheduling/settings/services/vehicle-category-service';
import { CustomerService } from '@/api/scheduling/billing/services/customer-service';
import { CheckoutSessionService } from '@/api/scheduling/billing/services/checkout-session-service';
import { PadSeriesService } from '@/api/product-catalog/pad-series/services/pad-series-service';
import { BrandService } from '@/api/product-catalog/brands/services/brand-service';
import { ReviewService } from './product-catalog/reviews/services/review-service';

export const api = {
    authentication: new AuthenticationService(),
    user: new UserService(),
    scheduling: {
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
    },
    productCatalog: {
        padSeries: new PadSeriesService(),
        brand: new BrandService(),
        review: new ReviewService()
    }
};
