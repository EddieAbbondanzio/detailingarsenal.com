import { authGuard } from '@/core/router/auth-guard';
import { adminGuard } from '@/modules/admin/router/admin-guard';
import { RouteConfig } from 'vue-router';

const adminRoutes: RouteConfig[] = [
    {
        path: '',
        name: 'adminPanel',
        component: () => import('@/modules/admin/views/admin-panel.vue')
    },
    {
        path: 'permissions',
        name: 'permissions',
        component: () => import('@/modules/admin/views/permission/permissions.vue')
    },
    {
        path: 'permissions/create',
        name: 'createPermission',
        component: () => import('@/modules/admin/views/permission/create-permission.vue')
    },
    {
        path: 'permission/:id/edit',
        name: 'editPermission',
        component: () => import('@/modules/admin/views/permission/edit-permission.vue')
    },
    {
        path: 'roles',
        name: 'roles',
        component: () => import('@/modules/admin/views/role/roles.vue')
    },
    {
        path: 'roles/create',
        name: 'createRole',
        component: () => import('@/modules/admin/views/role/create-role.vue')
    },
    {
        path: 'roles/:id',
        name: 'role',
        component: () => import('@/modules/admin/views/role/role.vue')
    },
    {
        path: 'roles/:id/edit',
        name: 'editRole',
        component: () => import('@/modules/admin/views/role/edit-role.vue')
    },
    {
        path: 'subscription-plans',
        name: 'subscriptionPlans',
        component: () => import('@/modules/admin/views/subscription-plan/subscription-plans.vue')
    },
    {
        path: 'subscription-plans/:id',
        name: 'subscriptionPlan',
        component: () => import('@/modules/admin/views/subscription-plan/subscription-plan.vue')
    }
];

export const admin = adminRoutes.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
