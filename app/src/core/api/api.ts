import { AuthService } from '@/modules/user/api/services/auth-service';
import { BusinessService } from '@/modules/settings/api/services/business-service';
import { HoursOfOperationService } from '@/modules/settings/api/services/hours-of-operation-service';
import { ServiceService } from '@/modules/settings/api/services/service-service';
import { VehicleCategoryService } from '@/modules/settings/api/services/vehicle-category-service';
import { UserService } from '@/modules/user/api/services/user-service';
import { ClientService } from '@/modules/clients/api/services/client-service';
import { AppointmentService } from '@/modules/calendar/api/service/appointment-service';

export const api = {
    auth: new AuthService(),
    user: new UserService(),
    appointment: new AppointmentService(),
    client: new ClientService(),
    settings: {
        business: new BusinessService(),
        hoursOfOperation: new HoursOfOperationService(),
        service: new ServiceService(),
        vehicleCategory: new VehicleCategoryService()
    }
};