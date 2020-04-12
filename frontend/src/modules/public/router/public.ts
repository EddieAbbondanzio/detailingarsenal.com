export const publics = {
    path: '/',
    component: () => import('@/modules/public/views/public.vue'),
    children: [
        {
            path: '/',
            name: 'landing',
            component: () => import('@/modules/public/views/landing.vue')
        },
        {
            path: '/about-us',
            name: 'aboutUs',
            component: () => import('@/modules/public/views/about-us.vue')
        },
        {
            path: '/pricing',
            name: 'pricing',
            component: () => import('@/modules/public/views/pricing.vue')
        },
        {
            path: '/contact',
            name: 'contact',
            component: () => import('@/modules/public/views/contact.vue')
        }
    ]
};
