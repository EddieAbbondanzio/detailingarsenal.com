import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { adminGuard } from '@/modules/admin/core/router/admin-guard';

const schedulingAdminRoutes: RouteConfig[] = [
    {
        path: 'scheduling/scheduling',
        name: 'schedulingPanel',
        component: () => import('@/modules/admin/scheduling/views/scheduling-panel.vue')
    },
    {
        path: 'scheduling/permissions',
        name: 'permissions',
        component: () => import('@/modules/admin/scheduling/views/permission/permissions.vue')
    },
    {
        path: 'scheduling/permissions/create',
        name: 'createPermission',
        component: () => import('@/modules/admin/scheduling/views/permission/create-permission.vue')
    },
    {
        path: 'scheduling/permission/:id/edit',
        name: 'editPermission',
        component: () => import('@/modules/admin/scheduling/views/permission/edit-permission.vue')
    },
    {
        path: 'scheduling/roles',
        name: 'roles',
        component: () => import('@/modules/admin/scheduling/views/role/roles.vue')
    },
    {
        path: 'scheduling/roles/create',
        name: 'createRole',
        component: () => import('@/modules/admin/scheduling/views/role/create-role.vue')
    },
    {
        path: 'scheduling/roles/:id',
        name: 'role',
        component: () => import('@/modules/admin/scheduling/views/role/role.vue')
    },
    {
        path: 'scheduling/roles/:id/edit',
        name: 'editRole',
        component: () => import('@/modules/admin/scheduling/views/role/edit-role.vue')
    },
    {
        path: 'scheduling/subscription-plans',
        name: 'subscriptionPlans',
        component: () => import('@/modules/admin/scheduling/views/subscription-plan/subscription-plans.vue')
    },
    {
        path: 'scheduling/subscription-plans/:id',
        name: 'subscriptionPlan',
        component: () => import('@/modules/admin/scheduling/views/subscription-plan/subscription-plan.vue')
    }
];

export const schedulingAdmin = schedulingAdminRoutes.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
