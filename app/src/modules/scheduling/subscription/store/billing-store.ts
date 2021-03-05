import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { stripe } from '@/plugins/stripe';
import { SubscriptionPlan, Customer, customerService, checkoutSessionService } from '@/api/scheduling';
import { subscriptionPlanService } from '@/api/scheduling';

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
            subscriptionPlanService.getDefault(),
            customerService.getCustomer()
        ]);

        this.context.commit('SET_DEFAULT_PLAN', plan);
        this.context.commit('SET_CUSTOMER', customer);
    }

    @Action({ rawError: true })
    async createCheckoutSession(priceBillingId: string) {
        var id = await checkoutSessionService.createSession(priceBillingId);
        await stripe.redirectToCheckout({ sessionId: id });
    }

    @Action({ rawError: true })
    async cancelSubscriptionAtPeriodEnd() {
        await customerService.cancelSubscription();
        this.context.commit('MARK_AS_CANCELLING');
    }

    @Action({ rawError: true })
    async undoCancellingAtPeriodEnd() {
        await customerService.undoCancellingSubscription();
        this.context.commit('REMOVE_CANCELLING');
    }
}

export default getModule(BillingStore);
