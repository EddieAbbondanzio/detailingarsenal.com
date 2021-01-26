import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import { calendar } from '@/modules/scheduling/calendar/router/calendar';
import { settings } from '@/modules/scheduling/settings/router/settings';
import { clients } from '@/modules/scheduling/clients/router/clients';
import { authGuard } from '@/core/router/auth-guard';
import { productCatalogRoutes } from '@/modules/product-catalog/core/router/product-catalog-routes';
import { adminRoutes } from '@/modules/admin/core/router/admin-routes';
import { profileRoutes } from '@/modules/user/profile/router/profile-routes';
import { schedulingRoutes } from '@/modules/scheduling/core/router/scheduling-routes';
import { userRoutes } from '@/modules/user/core/router/user-routes';

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    ...schedulingRoutes,
    ...userRoutes,
    ...adminRoutes,
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
    mode: 'history'
});

export default router;
