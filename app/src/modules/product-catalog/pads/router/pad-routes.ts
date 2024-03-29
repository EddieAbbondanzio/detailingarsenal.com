import { authGuard } from '@/core/router/auth-guard';

/**
 * Pad sub mobule specific routes
 */
export const padRoutes = [
    {
        path: 'pads',
        name: 'pads',
        component: () => import('@/modules/product-catalog/pads/views/pads.vue')
    },
    {
        path: 'pads/:padId',
        name: 'pad',
        component: () => import('@/modules/product-catalog/pads/views/pad.vue')
    },
    {
        path: 'pads/:padId/write-review',
        name: 'writeReview',
        component: () => import('@/modules/product-catalog/pads/views/write-review.vue'),
        beforeEnter: authGuard
    }
];
