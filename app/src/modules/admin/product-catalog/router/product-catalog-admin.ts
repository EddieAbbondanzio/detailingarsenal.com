import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { adminGuard } from '@/modules/admin/core/router/admin-guard';

const productCatalogAdminRoutes: RouteConfig[] = [
    {
        path: 'product-catalog',
        name: 'productCatalogPanel',
        component: () => import('@/modules/admin/product-catalog/views/product-catalog-panel.vue')
    },
    {
        path: 'product-catalog/brands',
        name: 'brands',
        component: () => import('@/modules/admin/product-catalog/views/brand/brands.vue')
    }
];

export const productCatalogAdmin = productCatalogAdminRoutes.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
