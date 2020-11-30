import { authGuard } from '@/core/router/auth-guard';
import { RouteConfig } from 'vue-router';
import { adminGuard } from '@/modules/admin/core/router/admin-guard';

const schedulingAdminRoutes: RouteConfig[] = [
    {
        path: 'scheduling',
        name: 'schedulingPanel',
        component: () => import('@/modules/admin/scheduling/views/scheduling-panel.vue')
    },
    {
        path: 'scheduling/subscription-plans',
        name: 'subscriptionPlans',
        component: () => import('@/modules/admin/scheduling/views/subscription-plan/subscription-plans.vue')
    },
    {
        path: 'scheduling/subscription-plans/:id',
        name: 'subscriptionPlan',
        component: () => import('@/modules/admin/scheduling/views/subscription-plan/subscription-plan.vue')
    },
    {
        path: 'scheduling/subscription-plans/:id/edit',
        name: 'editSubscriptionPlan',
        component: () => import('@/modules/admin/scheduling/views/subscription-plan/edit-subscription-plan.vue')
    }
];

export const schedulingAdmin = schedulingAdminRoutes.map(a => {
    a.beforeEnter = adminGuard;
    return a;
});
