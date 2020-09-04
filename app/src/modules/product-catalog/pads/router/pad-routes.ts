/**
 * Pad sub mobule specific routes
 */
export const padRoutes = [
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