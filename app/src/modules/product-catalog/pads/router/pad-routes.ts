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
        path: 'pads/:id/size/:size',
        name: 'pad',
        component: () => import('@/modules/product-catalog/pads/views/pad.vue')
    },
    {
        path: 'pads/:id/write-review',
        name: 'writeReview',
        component: () => import('@/modules/product-catalog/pads/views/write-review.vue')
    }
];