import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import { calendar } from '@/modules/scheduling/calendar/router/calendar';
import { settings } from '@/modules/scheduling/settings/router/settings';
import { user } from '@/modules/scheduling/user/router/user';
import { clients } from '@/modules/scheduling/clients/router/clients';
import { authGuard } from '@/core/router/auth-guard';
import { admin } from '@/modules/admin/router/admin';

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
        path: '/admin',
        component: () => import('@/core/views/private.vue'),
        children: [...admin],
        beforeEnter: authGuard // TODO: Refactor this. We need both authGuard, and adminGuard else user is never loaded
    },
    {
        path: '/goodbye',
        component: () => import('@/core/views/goodbye.vue')
    }
];

const router = new VueRouter({
    routes,
    linkActiveClass: 'is-active',
    mode: 'hash'
});

export default router;
