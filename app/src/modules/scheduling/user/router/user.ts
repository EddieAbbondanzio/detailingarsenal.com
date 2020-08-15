import { authGuard } from '@/core/router/auth-guard';

export const user = [
    {
        path: 'scheduling/account',
        name: 'account',
        component: () => import('@/modules/scheduling/user/views/account.vue')
    },
    {
        path: 'scheduling/account/profile',
        name: 'profile',
        component: () => import('@/modules/scheduling/user/views/profile.vue')
    },
    {
        path: 'scheduling/account/profile/edit',
        name: 'editProfile',
        component: () => import('@/modules/scheduling/user/views/edit-profile.vue')
    },
    {
        path: 'scheduling/account/subscription',
        name: 'subscription',
        component: () => import('@/modules/scheduling/user/views/subscription.vue')
    }
];
