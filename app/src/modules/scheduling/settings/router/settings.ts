import { authGuard } from '@/core/router/auth-guard';

export const settings = [
    {
        path: 'scheduling/settings/services',
        name: 'services',
        component: () => import('@/modules/scheduling/settings/views/services/services.vue')
    },
    {
        path: 'scheduling/settings/services/:id',
        name: 'service',
        component: () => import('@/modules/scheduling/settings/views/services/service.vue')
    },
    {
        path: 'scheduling/settings/services/:id/edit',
        name: 'editService',
        component: () => import('@/modules/scheduling/settings/views/services/edit-service.vue')
    },
    {
        path: 'scheduling/settings/service/create',
        name: 'createService',
        component: () => import('@/modules/scheduling/settings/views/services/create-service.vue')
    },
    {
        path: 'scheduling/settings',
        name: 'settings',
        component: () => import('@/modules/scheduling/settings/views/settings.vue')
    },
    {
        path: 'scheduling/settings/vehicle-categories',
        name: 'vehicleCategories',
        component: () => import('@/modules/scheduling/settings/views/vehicle-categories/vehicle-categories.vue')
    },
    {
        path: 'scheduling/settings/vehicle-category/create',
        name: 'createVehicleCategory',
        component: () => import('@/modules/scheduling/settings/views/vehicle-categories/create-vehicle-category.vue')
    },
    {
        path: 'scheduling/settings/vehicle-categories/:id/edit',
        name: 'editVehicleCategory',
        component: () => import('@/modules/scheduling/settings/views/vehicle-categories/edit-vehicle-category.vue')
    },
    {
        path: 'scheduling/settings/hours-of-operation',
        name: 'hoursOfOperation',
        component: () => import('@/modules/scheduling/settings/views/hours-of-operation/hours-of-operation.vue')
    },
    {
        path: 'scheduling/settings/hours-of-operation/edit',
        name: 'editHoursOfOperation',
        component: () => import('@/modules/scheduling/settings/views/hours-of-operation/edit-hours-of-operation.vue')
    },
    {
        path: 'scheduling/settings/business',
        name: 'business',
        component: () => import('@/modules/scheduling/settings/views/business/business.vue')
    },
    {
        path: 'scheduling/settings/business/edit',
        name: 'editBusiness',
        component: () => import('@/modules/scheduling/settings/views/business/edit-business.vue')
    }
];
