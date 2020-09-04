import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import { calendar } from '@/modules/scheduling/calendar/router/calendar';
import { settings } from '@/modules/scheduling/settings/router/settings';
import { user } from '@/modules/user/router/user';
import { clients } from '@/modules/scheduling/clients/router/clients';
import { authGuard } from '@/core/router/auth-guard';
import { productCatalogRoutes } from '@/modules/product-catalog/core/router/product-catalog-routes';
import { admin } from '@/modules/admin/core/router/admin';

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    {
        path: '/scheduling',
        component: () => import('@/core/views/private.vue'),
        children: [...calendar, ...settings, ...user, ...clients],
        // redirect: 'calendar',
        beforeEnter: authGuard
    },
    {
        path: '/goodbye',
        component: () => import('@/core/views/goodbye.vue')
    },
    ...admin,
    ...productCatalogRoutes,
    {
        // Wild card always has to go last.
        path: '*',
        component: () => import('@/core/views/404.vue')
    },
];

const router = new VueRouter({
    routes,
    linkActiveClass: 'is-active',
    mode: 'hash'
});

export default router;
