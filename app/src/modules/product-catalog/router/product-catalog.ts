import { authGuard } from '@/core/router/auth-guard';

export const productCatalog = [
    {
        path: 'product-catalog/pads',
        name: 'pads',
        component: () => import('@/modules/product-catalog/views/pads.vue')
    }
];
