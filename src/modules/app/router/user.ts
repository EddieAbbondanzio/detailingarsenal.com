import { authGuard } from '@/core/router/auth-guard';

export const user = [
    {
        path: 'user/settings',
        name: 'userSettings',
        component: () => import('@/modules/app/views/user/user-settings.vue')
    },
    {
        path: 'user/settings/edit',
        name: 'editUserSettings',
        component: () => import('@/modules/app/views/user/edit-user-settings.vue')
    }
];
