export const subscriptionRoutes = [
    {
        path: 'account/subscription',
        name: 'subscription',
        component: () => import('@/modules/scheduling/user/views/subscription.vue')
    }
];