import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { adminGuard } from '@/modules/admin/core/router/admin-guard';

const adminRoutes: RouteConfig[] = [
    {
        path: '',
        name: 'adminPanel',
        component: () => import('@/modules/admin/core/views/admin-panel.vue')
    }
];

export const admin = adminRoutes.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
