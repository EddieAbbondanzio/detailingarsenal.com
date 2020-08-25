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
    },
    {
        path: '/product-catalog/brands/create',
        name: 'createBrand',
        component: () => import('@/modules/admin/product-catalog/views/brand/create-brand.vue')
    },
    {
        path: '/product-catalog/brands/:id/edit',
        name: 'editBrand',
        component: () => import('@/modules/admin/product-catalog/views/brand/edit-brand.vue')
    },
    {
        path: '/product-catalog/pad-series',
        name: 'padSeries',
        component: () => import('@/modules/admin/product-catalog/views/pad-series/pad-series.vue')
    },
    {
        path: '/product-catalog/pads-series/create',
        name: 'createPadSeries',
        component: () => import('@/modules/admin/product-catalog/views/pad-series/create-pad-series.vue')
    },
    {
        path: '/product-catalog/pad-series/:id',
        name: 'padSeriesDetails',
        component: () => import('@/modules/admin/product-catalog/views/pad-series/pad-series-details.vue')
    }
];

export const productCatalogAdmin = productCatalogAdminRoutes.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
