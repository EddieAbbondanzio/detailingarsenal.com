import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { VehicleCategory } from '@/modules/app/api/settings/entities/vehicle-category';
import { Employee } from '@/modules/app/api/settings/entities/employee';
import { Business } from '@/modules/app/api/settings/entities/business';
import { HoursOfOperation } from '@/modules/app/api/settings/entities/hours-of-operation';
import { Service } from '@/modules/app/api/settings/entities/service';
import { UpdateBusiness } from '@/modules/app/api/settings/data-transfer-objects/update-business';
import { CreateVehicleCategory } from '@/modules/app/api/settings/data-transfer-objects/create-vehicle-category';
import { UpdateVehicleCategory } from '@/modules/app/api/settings/data-transfer-objects/update-vehicle-category';
import { CreateEmployee } from '@/modules/app/api/settings/data-transfer-objects/create-employee';
import { UpdateEmployee } from '@/modules/app/api/settings/data-transfer-objects/update-employee';
import { CreateService } from '@/modules/app/api/settings/data-transfer-objects/create-service';
import { UpdateService } from '@/modules/app/api/settings/data-transfer-objects/update-service';
import { api } from '@/modules/app/api/api';
import { InitableModule } from '@/core/store/initable-module';
import { UpdateHoursOfOperation } from '@/modules/app/api/settings/data-transfer-objects/update-hours-of-operation';
import store from '@/core/store/index';

/**
 * Store for the service view.
 */
@Module({ namespaced: true, name: 'settings', dynamic: true, store })
class SettingsStore extends InitableModule {
    /**
     * Vehicle categories used by the user to organize price / time.
     */
    public vehicleCategories: VehicleCategory[] = [];

    public employees: Employee[] = [];

    public business: Business = null!;

    public hoursOfOperation: HoursOfOperation = null!;

    /**
     * Service archetypes to create new jobs with.
     */
    public services: Service[] = [];

    @Mutation
    public SET_VEHICLE_CATEGORIES(vehicleCategories: VehicleCategory[]) {
        this.vehicleCategories = vehicleCategories;
    }

    @Mutation
    public CREATE_VEHICLE_CATEGORY(vehicleCategory: VehicleCategory) {
        this.vehicleCategories.push(vehicleCategory);
    }

    @Mutation
    public UPDATE_VEHICLE_CATEGORY(vehicleCategory: VehicleCategory) {
        this.vehicleCategories = [...this.vehicleCategories.filter(vc => vc.id != vehicleCategory.id), vehicleCategory];
    }

    @Mutation
    public DELETE_VEHICLE_CATEGORY(vehicleCategory: VehicleCategory) {
        const index = this.vehicleCategories.findIndex(vc => vc.id == vehicleCategory.id);
        this.vehicleCategories.splice(index, 1);
    }

    @Mutation
    public SET_EMPLOYEES(employees: Employee[]) {
        this.employees = employees;
    }

    @Mutation
    public CREATE_EMPLOYEE(employee: Employee) {
        this.employees.push(employee);
    }

    @Mutation
    public UPDATE_EMPLOYEE(employee: Employee) {
        this.employees = [...this.employees.filter(e => e.id != employee.id), employee];
    }

    @Mutation
    public DELETE_EMPLOYEE(employee: Employee) {
        const index = this.employees.findIndex(e => e.id == employee.id);
        this.employees.splice(index, 1);
    }

    @Mutation
    public SET_BUSINESS(business: Business) {
        this.business = business;
    }

    @Mutation
    public SET_HOURS_OF_OPERATION(hoursOfOperation: HoursOfOperation) {
        this.hoursOfOperation = hoursOfOperation;
    }

    @Mutation
    public CREATE_SERVICE(service: Service) {
        this.services.push(service);
    }

    @Mutation
    public UPDATE_SERVICE(service: Service) {
        this.services = [...this.services.filter(s => s.id != service.id), service];
    }

    @Mutation
    public DELETE_SERVICE(service: Service) {
        const index = this.services.findIndex(s => s.id == service.id);

        if (index != -1) {
            this.services.splice(index, 1);
        }
    }

    @Mutation
    public SET_SERVICES(services: Service[]) {
        this.services = services;
    }

    /**
     * Load the settings from the API.
     */
    @Action({ rawError: true })
    async _init() {
        const [business, vehicleCategories, hoursOfOp, services] = await Promise.all([
            api.settings.business.getBusiness(),
            api.settings.vehicleCategory.getVehicleCategories(),
            // api.settings.employee.getEmployees(),
            api.settings.hoursOfOperation.getHoursOfOperation(),
            api.settings.service.getServices()
        ]);

        this.context.commit('SET_BUSINESS', business);
        this.context.commit('SET_VEHICLE_CATEGORIES', vehicleCategories);
        // this.context.commit('SET_EMPLOYEES', employees);
        this.context.commit('SET_HOURS_OF_OPERATION', hoursOfOp);
        this.context.commit('SET_SERVICES', services);
    }

    @Action({ rawError: true })
    public async createVehicleCategory(createVehicleCategory: CreateVehicleCategory) {
        var vc = await api.settings.vehicleCategory.createVehicleCategory(createVehicleCategory);

        this.context.commit('CREATE_VEHICLE_CATEGORY', vc);
    }

    @Action({ rawError: true })
    public async updateVehicleCategory(updateVehicleCategory: UpdateVehicleCategory) {
        var vc = await api.settings.vehicleCategory.updateVehicleCategory(updateVehicleCategory);

        this.context.commit('UPDATE_VEHICLE_CATEGORY', vc);
    }

    /**
     * Delete a vehicle category.
     * @param vehicleCategory The vehicle category to delete.
     */
    @Action({ rawError: true })
    public async deleteVehicleCategory(vehicleCategory: VehicleCategory) {
        var vc = await api.settings.vehicleCategory.deleteVehicleCategory(vehicleCategory);

        this.context.commit('DELETE_VEHICLE_CATEGORY', vehicleCategory);
    }

    /**
     * Create a new employee and notify the backend.
     * @param employee The employee to create
     */
    @Action({ rawError: true })
    public async createEmployee(createEmployee: CreateEmployee) {
        var e = await api.settings.employee.createEmployee(createEmployee);
        this.context.commit('CREATE_EMPLOYEE', e);
    }

    /**
     * update the details of an employee and notify the backend.
     * @param employee The employee to update.
     */
    @Action({ rawError: true })
    public async updateEmployee(updateEmployee: UpdateEmployee) {
        var e = await api.settings.employee.updateEmployee(updateEmployee);
        this.context.commit('UPDATE_EMPLOYEE', e);
    }

    /**
     * Delete an employee from the backend.
     * @param employee The employee to delete.
     */
    @Action({ rawError: true })
    public async deleteEmployee(employee: Employee) {
        await api.settings.employee.deleteEmployee(employee);
        this.context.commit('DELETE_EMPLOYEE', employee);
    }

    @Action({ rawError: true })
    public async updateBusiness(updateBusiness: UpdateBusiness) {
        const b = await api.settings.business.updateBusiness(updateBusiness);
        this.context.commit('SET_BUSINESS', b);
    }

    @Action({ rawError: true })
    /**
     * Set the business hours.
     * @param hoursOfOperation The hours to set.
     */
    @Action({ rawError: true })
    public async updateHoursOfOperation(update: UpdateHoursOfOperation) {
        const hours = await api.settings.hoursOfOperation.updateHoursOfOperation(update);
        this.context.commit('SET_HOURS_OF_OPERATION', hours);
    }

    @Action({ rawError: true })
    public async createService(createService: CreateService) {
        const service = await api.settings.service.createService(createService);
        this.context.commit('CREATE_SERVICE', service);
    }

    @Action({ rawError: true })
    public async updateService(updateService: UpdateService) {
        const service = await api.settings.service.updateService(updateService);
        this.context.commit('UPDATE_SERVICE', service);
    }

    @Action({ rawError: true })
    public async deleteService(service: Service) {
        api.settings.service.deleteService(service.id);
        this.context.commit('DELETE_SERVICE', service);
    }
}

export default getModule(SettingsStore);
