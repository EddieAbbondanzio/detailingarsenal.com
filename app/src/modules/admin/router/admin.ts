import { authGuard } from '@/core/router/auth-guard';

export const admin = [
    {
        path: 'admin',
        name: 'adminPanel',
        component: () => import('@/modules/admin/views/admin-panel.vue')
    },
    {
        path: 'admin/permissions',
        name: 'permissions',
        component: () => import('@/modules/admin/views/permission/permissions.vue')
    },
    {
        path: 'admin/permissions/create',
        name: 'createPermission',
        component: () => import('@/modules/admin/views/permission/create-permission.vue')
    },
    {
        path: 'admin/permission/:id/edit',
        name: 'editPermission',
        component: () => import('@/modules/admin/views/permission/edit-permission.vue')
    },
    {
        path: 'admin/roles',
        name: 'roles',
        component: () => import('@/modules/admin/views/role/roles.vue')
    },
    {
        path: 'admin/roles/create',
        name: 'createRole',
        component: () => import('@/modules/admin/views/role/create-role.vue')
    },
    {
        path: 'admin/roles/:id/edit',
        name: 'editRole',
        component: () => import('@/modules/admin/views/role/edit-role.vue')
    }
];
