import { authGuard } from '@/core/router/auth-guard';

export const user = [
    {
        path: 'user/settings',
        name: 'userSettings',
        component: () => import('@/modules/user/views/user-settings.vue')
    },
    {
        path: 'user/settings/edit',
        name: 'editUserSettings',
        component: () => import('@/modules/user/views/edit-user-settings.vue')
    }
];
