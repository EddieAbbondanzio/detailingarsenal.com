import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { adminGuard } from '@/modules/admin/core/router/admin-guard';
import { schedulingAdmin } from '../../scheduling/router/scheduling-admin';
import { productCatalogAdmin } from '../../product-catalog/router/product-catalog-admin';
import { usersAdmin } from '../../users/router/users-admin';

const a: RouteConfig[] = [
    {
        path: '/admin',
        component: () => import('@/modules/admin/core/views/admin-parent-view.vue'),
        children: [
            {
                path: '',
                name: 'adminPanel',
                component: () => import('@/modules/admin/core/views/admin-panel.vue'),
            },
            ...productCatalogAdmin,
            ...schedulingAdmin,
            ...usersAdmin,

        ],
    },
];

export const adminRoutes = a.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
