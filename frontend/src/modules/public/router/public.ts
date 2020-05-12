export const publics = {
    path: '/',
    component: () => import('@/modules/public/views/public.vue'),
    children: [
        {
            path: '/',
            name: 'landing',
            component: () => import('@/modules/public/views/landing.vue')
        }
    ]
};
