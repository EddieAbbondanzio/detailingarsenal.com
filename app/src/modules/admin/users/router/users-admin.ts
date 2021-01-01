import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { adminGuard } from '@/modules/admin/core/router/admin-guard';

const usersAdminRoutes: RouteConfig[] = [
    {
        path: 'users/users',
        name: 'usersPanel',
        component: () => import('@/modules/admin/users/views/users-panel.vue')
    },
    {
        path: 'users/permissions',
        name: 'permissions',
        component: () => import('@/modules/admin/users/views/permission/permissions.vue')
    },
    {
        path: 'users/permissions/create',
        name: 'createPermission',
        component: () => import('@/modules/admin/users/views/permission/create-or-update-permission.vue')
    },
    {
        path: 'users/permission/:id/update',
        name: 'updatePermission',
        component: () => import('@/modules/admin/users/views/permission/create-or-update-permission.vue')
    },
    {
        path: 'users/roles',
        name: 'roles',
        component: () => import('@/modules/admin/users/views/role/roles.vue')
    },
    {
        path: 'users/roles/create',
        name: 'createRole',
        component: () => import('@/modules/admin/users/views/role/create-or-update-role.vue')
    },
    {
        path: 'users/roles/:id',
        name: 'role',
        component: () => import('@/modules/admin/users/views/role/role.vue')
    },
    {
        path: 'users/roles/:id/update',
        name: 'updateRole',
        component: () => import('@/modules/admin/users/views/role/create-or-update-role.vue')
    },

];

export const usersAdmin = usersAdminRoutes.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
