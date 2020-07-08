import { Module, VuexModule, Mutation, Action } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { SubscriptionPlan } from '@/modules/admin/api/entities/subscription-plan';
import store from '@/core/store/index';
import { api } from '@/core/api/api';

@Module({ namespaced: true, name: 'billing', dynamic: true, store })
class BillingStore extends InitableModule {
    public defaultPlan: SubscriptionPlan = null!;

    @Mutation
    SET_DEFAULT_PLAN(plan: SubscriptionPlan) {
        this.defaultPlan = plan;
    }

    @Action({ rawError: true })
    async _init() {
        const plan = await api.billing.subscriptionPlan.getDefault();
        this.context.commit('SET_DEFAULT_PLAN', plan);
    }
}

export default getModule(BillingStore);
