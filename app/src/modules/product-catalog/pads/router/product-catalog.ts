import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';

export const productCatalog: RouteConfig[] = [
    {
        path: '/product-catalog',
        component: () => import('@/modules/product-catalog/core/views/product-catalog.vue'),
        children: [
            {
                path: 'pads/:id',
                name: 'pad',
                component: () => import('@/modules/product-catalog/pads/views/pad.vue')
            },
            {
                path: 'pads',
                name: 'pads',
                component: () => import('@/modules/product-catalog/pads/views/pads.vue')
            },
        ]
    }
];
