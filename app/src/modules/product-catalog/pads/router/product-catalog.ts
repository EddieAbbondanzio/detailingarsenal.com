import { authGuard } from '@/core/router/auth-guard';

export const productCatalog = [
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

];
