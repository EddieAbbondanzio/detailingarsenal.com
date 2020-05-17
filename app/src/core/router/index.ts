import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import { app } from '@/modules/app/router/app';

Vue.use(VueRouter);

const routes: RouteConfig[] = [app];

const router = new VueRouter({
    routes,
    linkActiveClass: 'is-active',
    mode: 'history'
});

export default router;
