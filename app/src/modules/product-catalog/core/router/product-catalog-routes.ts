import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { padRoutes } from '../../pads/router/pad-routes';

export const productCatalogRoutes: RouteConfig[] = [
    {
        path: '/product-catalog',
        component: () => import('@/modules/product-catalog/core/views/product-catalog-parent-view.vue'),
        children: [
            ...padRoutes
        ]
    }
];
