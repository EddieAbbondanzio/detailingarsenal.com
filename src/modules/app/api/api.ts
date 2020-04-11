import { AuthService } from '@/modules/app/api/user/services/auth-service';
import { BusinessService } from '@/modules/app/api/settings/services/business-service';
import { EmployeeService } from '@/modules/app/api/settings/services/employee-service';
import { HoursOfOperationService } from '@/modules/app/api/settings/services/hours-of-operation-service';
import { ServiceService } from '@/modules/app/api/settings/services/service-service';
import { VehicleCategoryService } from '@/modules/app/api/settings/services/vehicle-category-service';
import { UserService } from '@/modules/app/api/user/services/user-service';
import { ClientService } from '@/modules/app/api/clients/services/client-service';
import { AppointmentService } from '@/modules/app/api/calendar/service/appointment-service';

export const api = {
    auth: new AuthService(),
    user: new UserService(),
    appointment: new AppointmentService(),
    client: new ClientService(),
    settings: {
        business: new BusinessService(),
        employee: new EmployeeService(),
        hoursOfOperation: new HoursOfOperationService(),
        service: new ServiceService(),
        vehicleCategory: new VehicleCategoryService()
    }
};
