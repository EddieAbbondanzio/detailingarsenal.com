import { profileRoutes } from '@/modules/user/profile/router/profile-routes';
import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';

export const userRoutes: RouteConfig[] = [
    {
        path: '/user',
        component: () => import('@/modules/user/core/views/user-parent-view.vue'),
        children: [
            {
                path: '',
                name: 'user',
                component: () => import('@/modules/user/core/views/user.vue')
            },
            ...profileRoutes
        ],
        beforeEnter: authGuard
    }
];