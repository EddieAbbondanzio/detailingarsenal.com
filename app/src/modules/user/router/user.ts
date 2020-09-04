import { authGuard } from '@/core/router/auth-guard';

export const user = [
    {
        path: 'account',
        name: 'account',
        component: () => import('@/modules/user/views/account.vue')
    },
    {
        path: 'account/profile',
        name: 'profile',
        component: () => import('@/modules/user/views/profile.vue')
    },
    {
        path: 'account/profile/edit',
        name: 'editProfile',
        component: () => import('@/modules/user/views/edit-profile.vue')
    },
];
