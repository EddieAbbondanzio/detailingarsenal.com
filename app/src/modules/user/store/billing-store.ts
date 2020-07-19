import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { SubscriptionPlan } from '@/api/billing/data-transfer-objects/subscription-plan';
import store from '@/core/store/index';
import { api } from '@/api/api';
import { Subscription, Customer } from '@/api';
import { stripe } from '@/plugins/stripe';

@Module({ namespaced: true, name: 'billing', dynamic: true, store })
class BillingStore extends InitableModule {
    defaultPlan: SubscriptionPlan = null!;
    customer: Customer = null!;

    @Mutation
    SET_DEFAULT_PLAN(plan: SubscriptionPlan) {
        this.defaultPlan = plan;
    }

    @Mutation
    SET_CUSTOMER(customer: Customer) {
        this.customer = customer;
    }

    @Action({ rawError: true })
    async _init() {
        const [plan, customer] = await Promise.all([
            api.billing.subscriptionPlan.getDefault(),
            api.billing.customer.getCustomer()
        ]);

        this.context.commit('SET_DEFAULT_PLAN', plan);
        this.context.commit('SET_CUSTOMER', customer);
    }

    @Action({ rawError: true })
    async createCheckoutSession(priceBillingId: string) {
        var id = await api.billing.checkoutSession.createSession(priceBillingId);
        await stripe.redirectToCheckout({ sessionId: id });
    }
}

export default getModule(BillingStore);