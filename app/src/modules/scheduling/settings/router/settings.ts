import { authGuard } from '@/core/router/auth-guard';

export const settings = [
    {
        path: 'settings/services',
        name: 'services',
        component: () => import('@/modules/scheduling/settings/views/services/services.vue')
    },
    {
        path: 'settings/services/:id',
        name: 'service',
        component: () => import('@/modules/scheduling/settings/views/services/service.vue')
    },
    {
        path: 'settings/services/:id/edit',
        name: 'editService',
        component: () => import('@/modules/scheduling/settings/views/services/edit-service.vue')
    },
    {
        path: 'settings/service/create',
        name: 'createService',
        component: () => import('@/modules/scheduling/settings/views/services/create-service.vue')
    },
    {
        path: 'settings',
        name: 'settings',
        component: () => import('@/modules/scheduling/settings/views/settings.vue')
    },
    {
        path: 'settings/vehicle-categories',
        name: 'vehicleCategories',
        component: () => import('@/modules/scheduling/settings/views/vehicle-categories/vehicle-categories.vue')
    },
    {
        path: 'settings/vehicle-category/create',
        name: 'createVehicleCategory',
        component: () => import('@/modules/scheduling/settings/views/vehicle-categories/create-vehicle-category.vue')
    },
    {
        path: 'settings/vehicle-categories/:id/edit',
        name: 'editVehicleCategory',
        component: () => import('@/modules/scheduling/settings/views/vehicle-categories/edit-vehicle-category.vue')
    },
    {
        path: 'settings/hours-of-operation',
        name: 'hoursOfOperation',
        component: () => import('@/modules/scheduling/settings/views/hours-of-operation/hours-of-operation.vue')
    },
    {
        path: 'settings/hours-of-operation/edit',
        name: 'editHoursOfOperation',
        component: () => import('@/modules/scheduling/settings/views/hours-of-operation/edit-hours-of-operation.vue')
    },
    {
        path: 'settings/business',
        name: 'business',
        component: () => import('@/modules/scheduling/settings/views/business/business.vue')
    },
    {
        path: 'settings/business/edit',
        name: 'editBusiness',
        component: () => import('@/modules/scheduling/settings/views/business/edit-business.vue')
    }
];
