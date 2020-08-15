import { authGuard } from '@/core/router/auth-guard';

export const productCatalog = [
    {
        path: 'pads',
        name: 'pads',
        component: () => import('@/modules/product-catalog/views/pads.vue')
    }
];
