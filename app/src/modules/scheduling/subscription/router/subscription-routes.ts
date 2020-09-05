export const subscriptionRoutes = [
    {
        path: 'account/subscription',
        name: 'subscription',
        component: () => import('@/modules/scheduling/subscription/views/subscription.vue'),
    }
];