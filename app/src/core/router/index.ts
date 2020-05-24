import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import { calendar } from '@/modules/calendar/router/calendar';
import { settings } from '@/modules/settings/router/settings';
import { user } from '@/modules/user/router/user';
import { clients } from '@/modules/clients/router/clients';
import { authGuard } from '@/core/router/auth-guard';

Vue.use(VueRouter);

const routes: RouteConfig[] = [
    {
        path: '/',
        component: () => import('@/core/views/app.vue'),
        children: [...calendar, ...settings, ...user, ...clients],
        beforeEnter: authGuard
    },
    {
        path: '/login',
        component: () => import('@/core/views/login.vue')
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
