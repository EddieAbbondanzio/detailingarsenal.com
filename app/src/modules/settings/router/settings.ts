import { authGuard } from '@/core/router/auth-guard';

export const settings = [
    {
        path: 'services',
        name: 'services',
        component: () => import('@/modules/settings/views/services/services.vue')
    },
    {
        path: 'services/:id',
        name: 'service',
        component: () => import('@/modules/settings/views/services/service.vue')
    },
    {
        path: 'services/:id/edit',
        name: 'editService',
        component: () => import('@/modules/settings/views/services/edit-service.vue')
    },
    {
        path: 'service/create',
        name: 'createService',
        component: () => import('@/modules/settings/views/services/create-service.vue')
    },
    {
        path: 'settings',
        name: 'settings',
        component: () => import('@/modules/settings/views/settings.vue')
    },
    {
        path: 'settings/vehicle-categories',
        name: 'vehicleCategories',
        component: () => import('@/modules/settings/views/vehicle-categories/vehicle-categories.vue')
    },
    {
        path: 'settings/vehicle-category/create',
        name: 'createVehicleCategory',
        component: () => import('@/modules/settings/views/vehicle-categories/create-vehicle-category.vue')
    },
    {
        path: 'settings/vehicle-categories/:id/edit',
        name: 'editVehicleCategory',
        component: () => import('@/modules/settings/views/vehicle-categories/edit-vehicle-category.vue')
    },
    {
        path: 'settings/hours-of-operation',
        name: 'hoursOfOperation',
        component: () => import('@/modules/settings/views/hours-of-operation/hours-of-operation.vue')
    },
    {
        path: 'settings/hours-of-operation/edit',
        name: 'editHoursOfOperation',
        component: () => import('@/modules/settings/views/hours-of-operation/edit-hours-of-operation.vue')
    },
    {
        path: 'settings/business',
        name: 'business',
        component: () => import('@/modules/settings/views/business/business.vue')
    },
    {
        path: 'settings/business/edit',
        name: 'editBusiness',
        component: () => import('@/modules/settings/views/business/edit-business.vue')
    }
];
