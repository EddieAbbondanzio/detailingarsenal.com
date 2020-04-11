import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import { publics } from '@/modules/public/router/public';
import { app } from '@/modules/app/router/app';

Vue.use(VueRouter);

const routes: RouteConfig[] = [publics, app];

const router = new VueRouter({
    routes,
    linkActiveClass: 'is-active',
    mode: 'history'
});

export default router;
