import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { SubscriptionPlan, subscriptionPlanService, SubscriptionPlanUpdateRequest } from '@/api/scheduling';

@Module({ namespaced: true, name: 'subscription-plan', dynamic: true, store })
class SubscriptionPlanStore extends InitableModule {
    subscriptionPlans: SubscriptionPlan[] = [];

    @Mutation
    SET_SUBSCRIPTION_PLANS(plans: SubscriptionPlan[]) {
        this.subscriptionPlans = plans;
    }

    @Mutation
    UPDATE_SUBSCRIPTION_PLAN(plan: SubscriptionPlan) {
        this.subscriptionPlans = [...this.subscriptionPlans.filter(p => p.id != plan.id), plan];
    }

    @Action({ rawError: true })
    async _init() {
        const [plans] = await Promise.all([subscriptionPlanService.get()]);

        this.context.commit('SET_SUBSCRIPTION_PLANS', plans);
    }

    @Action({ rawError: true })
    async refreshSubscriptionPlans() {
        const plans = await subscriptionPlanService.refreshPlans();
        this.context.commit('SET_SUBSCRIPTION_PLANS', plans);
        return plans;
    }

    @Action({ rawError: true })
    async updateSubscriptionPlan(update: SubscriptionPlanUpdateRequest) {
        var p = await subscriptionPlanService.updatePlan(update);
        this.context.commit('UPDATE_SUBSCRIPTION_PLAN', p);

        return p;
    }
}

export default getModule(SubscriptionPlanStore);
