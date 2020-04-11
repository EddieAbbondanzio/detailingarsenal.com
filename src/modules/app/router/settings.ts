import { authGuard } from '@/core/router/auth-guard';

export const settings = [
    {
        path: 'services',
        name: 'services',
        component: () => import('@/modules/app/views/settings/services/services.vue')
    },
    {
        path: 'services/:id',
        name: 'service',
        component: () => import('@/modules/app/views/settings/services/service.vue')
    },
    {
        path: 'services/:id/edit',
        name: 'editService',
        component: () => import('@/modules/app/views/settings/services/edit-service.vue')
    },
    {
        path: 'service/create',
        name: 'createService',
        component: () => import('@/modules/app/views/settings/services/create-service.vue')
    },
    {
        path: 'settings',
        name: 'settings',
        component: () => import('@/modules/app/views/settings/settings.vue')
    },
    {
        path: 'settings/vehicle-categories',
        name: 'vehicleCategories',
        component: () => import('@/modules/app/views/settings/vehicle-categories/vehicle-categories.vue')
    },
    {
        path: 'settings/vehicle-category/create',
        name: 'createVehicleCategory',
        component: () => import('@/modules/app/views/settings/vehicle-categories/create-vehicle-category.vue')
    },
    {
        path: 'settings/vehicle-categories/:id/edit',
        name: 'editVehicleCategory',
        component: () => import('@/modules/app/views/settings/vehicle-categories/edit-vehicle-category.vue')
    },
    {
        path: 'settings/hours-of-operation',
        name: 'hoursOfOperation',
        component: () => import('@/modules/app/views/settings/hours-of-operation/hours-of-operation.vue')
    },
    {
        path: 'settings/hours-of-operation/edit',
        name: 'editHoursOfOperation',
        component: () => import('@/modules/app/views/settings/hours-of-operation/edit-hours-of-operation.vue')
    },
    {
        path: 'settings/business',
        name: 'business',
        component: () => import('@/modules/app/views/settings/business/business.vue')
    },
    {
        path: 'settings/business/edit',
        name: 'editBusiness',
        component: () => import('@/modules/app/views/settings/business/edit-business.vue')
    }
];
