import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';

import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import {
    VehicleCategory,
    Business,
    HoursOfOperation,
    Service,
    VehicleCategoryCreate,
    VehicleCategoryUpdate,
    BusinessUpdate,
    HoursOfOperationUpdate,
    ServiceCreate,
    ServiceUpdate,
    businessService,
    vehicleCategoryService,
    hoursOfOperationService,
    serviceService
} from '@/api/scheduling';

/**
 * Store for the service view.
 */
@Module({ namespaced: true, name: 'settings', dynamic: true, store })
class SettingsStore extends InitableModule {
    /**
     * Vehicle categories used by the user to organize price / time.
     */
    public vehicleCategories: VehicleCategory[] = [];

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
     * Load the settings from the api.scheduling.
     */
    @Action({ rawError: true })
    async _init() {
        const [business, vehicleCategories, hoursOfOp, services] = await Promise.all([
            businessService.getBusiness(),
            vehicleCategoryService.getVehicleCategories(),
            hoursOfOperationService.getHoursOfOperation(),
            serviceService.getServices()
        ]);

        this.context.commit('SET_BUSINESS', business);
        this.context.commit('SET_VEHICLE_CATEGORIES', vehicleCategories);
        this.context.commit('SET_HOURS_OF_OPERATION', hoursOfOp);
        this.context.commit('SET_SERVICES', services);
    }

    @Action({ rawError: true })
    public async createVehicleCategory(createVehicleCategory: VehicleCategoryCreate) {
        var vc = await vehicleCategoryService.createVehicleCategory(createVehicleCategory);
        this.context.commit('CREATE_VEHICLE_CATEGORY', vc);
    }

    @Action({ rawError: true })
    public async updateVehicleCategory(updateVehicleCategory: VehicleCategoryUpdate) {
        var vc = await vehicleCategoryService.updateVehicleCategory(updateVehicleCategory);
        this.context.commit('UPDATE_VEHICLE_CATEGORY', vc);
    }

    /**
     * Delete a vehicle category.
     * @param vehicleCategory The vehicle category to delete.
     */
    @Action({ rawError: true })
    public async deleteVehicleCategory(vehicleCategory: VehicleCategory) {
        await vehicleCategoryService.deleteVehicleCategory(vehicleCategory);
        this.context.commit('DELETE_VEHICLE_CATEGORY', vehicleCategory);
    }

    @Action({ rawError: true })
    public async updateBusiness(updateBusiness: BusinessUpdate) {
        const b = await businessService.updateBusiness(updateBusiness);
        this.context.commit('SET_BUSINESS', b);
    }

    @Action({ rawError: true })
    /**
     * Set the business hours.
     * @param hoursOfOperation The hours to set.
     */
    @Action({ rawError: true })
    public async updateHoursOfOperation(update: HoursOfOperationUpdate) {
        const hours = await hoursOfOperationService.updateHoursOfOperation(update);
        this.context.commit('SET_HOURS_OF_OPERATION', hours);
    }

    @Action({ rawError: true })
    public async createService(createService: ServiceCreate) {
        const service = await serviceService.createService(createService);
        this.context.commit('CREATE_SERVICE', service);
    }

    @Action({ rawError: true })
    public async updateService(updateService: ServiceUpdate) {
        const service = await serviceService.updateService(updateService);
        this.context.commit('UPDATE_SERVICE', service);
    }

    @Action({ rawError: true })
    public async deleteService(service: Service) {
        await serviceService.deleteService(service.id);
        this.context.commit('DELETE_SERVICE', service);
    }
}

export default getModule(SettingsStore);
