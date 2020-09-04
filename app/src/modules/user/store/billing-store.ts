import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { SubscriptionPlan } from '@/api';
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

    @Mutation
    MARK_AS_CANCELLING() {
        if (this.customer.subscription == null) {
            return;
        }

        this.customer.subscription.cancellingAtPeriodEnd = true;
    }

    @Mutation
    REMOVE_CANCELLING() {
        if (this.customer.subscription == null) {
            return;
        }

        this.customer.subscription.cancellingAtPeriodEnd = false;
    }

    @Action({ rawError: true })
    async _init() {
        const [plan, customer] = await Promise.all([
            api.scheduling.billing.subscriptionPlan.getDefault(),
            api.scheduling.billing.customer.getCustomer()
        ]);

        this.context.commit('SET_DEFAULT_PLAN', plan);
        this.context.commit('SET_CUSTOMER', customer);
    }

    @Action({ rawError: true })
    async createCheckoutSession(priceBillingId: string) {
        var id = await api.scheduling.billing.checkoutSession.createSession(priceBillingId);
        await stripe.redirectToCheckout({ sessionId: id });
    }

    @Action({ rawError: true })
    async cancelSubscriptionAtPeriodEnd() {
        await api.scheduling.billing.customer.cancelSubscription();
        this.context.commit('MARK_AS_CANCELLING');
    }

    @Action({ rawError: true })
    async undoCancellingAtPeriodEnd() {
        await api.scheduling.billing.customer.undoCancellingSubscription();
        this.context.commit('REMOVE_CANCELLING');
    }
}

export default getModule(BillingStore);
