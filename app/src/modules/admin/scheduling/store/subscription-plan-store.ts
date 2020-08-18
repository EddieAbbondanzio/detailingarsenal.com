import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { api } from '@/api/api';
import store from '@/core/store/index';
import { SubscriptionPlan } from '@/api';

@Module({ namespaced: true, name: 'subscription-plan', dynamic: true, store })
class SubscriptionPlanStore extends InitableModule {
    subscriptionPlans: SubscriptionPlan[] = [];

    @Mutation
    SET_SUBSCRIPTION_PLANS(plans: SubscriptionPlan[]) {
        this.subscriptionPlans = plans;
    }

    @Action({ rawError: true })
    async _init() {
        const [plans] = await Promise.all([api.scheduling.billing.subscriptionPlan.getPlans()]);

        this.context.commit('SET_SUBSCRIPTION_PLANS', plans);
    }

    @Action({ rawError: true })
    async refreshSubscriptionPlans() {
        const plans = await api.scheduling.billing.subscriptionPlan.refreshPlans();
        this.context.commit('SET_SUBSCRIPTION_PLANS', plans);
        return plans;
    }
}

export default getModule(SubscriptionPlanStore);
