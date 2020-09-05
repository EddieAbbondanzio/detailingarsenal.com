import { authGuard } from '@/core/router/auth-guard';

export const profileRoutes = [
    {
        path: 'user/profile',
        name: 'profile',
        component: () => import('@/modules/user/profile/views/profile.vue'),
        beforeEnter: authGuard,
    },
    {
        path: 'user/profile/edit',
        name: 'editProfile',
        component: () => import('@/modules/user/profile/views/edit-profile.vue'),
        beforeEnter: authGuard,
    },
];
