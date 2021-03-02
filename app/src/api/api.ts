import { AppointmentService } from '@/api/scheduling/calendar/services/appointment-service';
import { SubscriptionPlanService } from '@/api/scheduling/billing/services/subscription-plan-service';
import { ClientService } from '@/api/scheduling/clients/services/client-service';
import { BusinessService } from '@/api/scheduling/settings/services/business-service';
import { HoursOfOperationService } from '@/api/scheduling/settings/services/hours-of-operation-service';
import { ServiceService } from '@/api/scheduling/settings/services/service-service';
import { VehicleCategoryService } from '@/api/scheduling/settings/services/vehicle-category-service';
import { CustomerService } from '@/api/scheduling/billing/services/customer-service';
import { CheckoutSessionService } from '@/api/scheduling/billing/services/checkout-session-service';
import { PadSeriesService } from '@/api/product-catalog/services/pad-series-service';
import { ReviewService } from './product-catalog/services/review-service';
import { UserService } from './user/common/services/user-service';
import { AuthenticationService } from './user/security/services/authentication-service';
import { PermissionService } from './user/security/services/permission-service';
import { RoleService } from './user/security/services/role-service';
import { BrandService } from './product-catalog/services/brand-service';
import { PadSeriesFilterService } from './product-catalog/services/pad-series-filter-service';
import { PadSummaryService } from './product-catalog/services/pad-summary-service';

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
        padSeriesFilter: new PadSeriesFilterService(),
        padSummary: new PadSummaryService(),
        brand: new BrandService(),
        review: new ReviewService()
    }
};
