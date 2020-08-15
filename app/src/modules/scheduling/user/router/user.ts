import { authGuard } from '@/core/router/auth-guard';

export const user = [
    {
        path: 'account',
        name: 'account',
        component: () => import('@/modules/scheduling/user/views/account.vue')
    },
    {
        path: 'account/profile',
        name: 'profile',
        component: () => import('@/modules/scheduling/user/views/profile.vue')
    },
    {
        path: 'account/profile/edit',
        name: 'editProfile',
        component: () => import('@/modules/scheduling/user/views/edit-profile.vue')
    },
    {
        path: 'account/subscription',
        name: 'subscription',
        component: () => import('@/modules/scheduling/user/views/subscription.vue')
    }
];
