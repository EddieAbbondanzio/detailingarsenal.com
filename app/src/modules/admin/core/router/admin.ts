import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { adminGuard } from '@/modules/admin/core/router/admin-guard';
import { schedulingAdmin } from '../../scheduling/router/scheduling-admin';
import { productCatalogAdmin } from '../../product-catalog/router/product-catalog-admin';

const adminRoutes: RouteConfig[] = [
    {
        path: '/admin',
        component: () => import('@/core/views/private.vue'),
        children: [
            {
                path: '',
                name: 'adminPanel',
                component: () => import('@/modules/admin/core/views/admin-panel.vue'),
            },
            ...productCatalogAdmin,
            ...schedulingAdmin],
    },
];

export const admin = adminRoutes.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
