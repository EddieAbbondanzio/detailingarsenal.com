<template>
    <page>
        <template v-slot:header>
            <page-header title="Subscription" description="Subscription plan and billing">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Account" :to="{ name: 'account' }" />
                        <breadcrumb
                            name="Subscription"
                            :to="{ name: 'subscription' }"
                            active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="customer != null">
            <p
                class="is-size-4 has-text-weight-bold has-text-centered has-margin-bottom-3"
            >One plan, no confusion.</p>

            <div class="card has-margin-top-3 has-margin-bottom-4">
                <!-- Trial Banner -->
                <div
                    v-if="state == 'trialing'"
                    class="has-background-warning has-w-100 has-padding-all-1 has-text-centered"
                >
                    Trialing. {{ customer.subscription.trialDaysRemaining }}
                    {{ customer.subscription.trialDaysRemaining == 1 ? 'day' : 'days' }} remaining.
                </div>

                <!-- Title -->
                <div class="has-background-primary has-padding-all-4">
                    <p
                        class="is-size-4 has-text-weight-bold has-text-white has-text-centered"
                    >Detailing Arsenal {{ name }}</p>
                </div>

                <div class="level has-padding-all-4">
                    <!-- Price -->
                    <div class="level-item">
                        <div>
                            <div class="is-flex is-flex-column is-align-items-center">
                                <p class="is-size-1" v-if="!showYearly">
                                    <small class="is-size-3" style="vertical-align: text-top;">$</small>
                                    {{ monthPrice }}
                                </p>
                                <p class="is-size-1" v-else>
                                    <small class="is-size-3" style="vertical-align: text-top;">$</small>
                                    {{ yearPrice }}
                                </p>
                                <p
                                    class="is-size-5 has-text-grey is-uppercase"
                                    style="align-self: bottom"
                                >/ {{ showYearly ? 'year' : 'month' }}</p>

                                <b-switch
                                    class="has-margin-y-3"
                                    type="is-info"
                                    v-model="showYearly"
                                    v-if="state != 'trialing_will_upgrade' && state != 'active'"
                                >
                                    Yearly
                                    <small class="has-text-grey">(2 months free!)</small>
                                </b-switch>
                            </div>
                        </div>
                    </div>

                    <!-- Feature List -->
                    <div class="level-item">
                        <div class="is-flex is-flex-column">
                            <ul class="has-margin-bottom-4">
                                <li>
                                    <b-icon
                                        icon="check"
                                        type="is-success"
                                        class="has-padding-right-3"
                                    />Unlimited
                                    appointment bookings
                                </li>
                                <li>
                                    <b-icon
                                        icon="check"
                                        type="is-success"
                                        class="has-padding-right-3"
                                    />Unlimited
                                    contacts
                                </li>
                                <li>
                                    <b-icon
                                        icon="check"
                                        type="is-success"
                                        class="has-padding-right-3"
                                    />Synchronized
                                    multi device support
                                </li>
                            </ul>

                            <div class="is-flex is-flex-row is-align-items-center">
                                <!-- Subscribe button -->
                                <div
                                    class="is-flex is-flex-row is-align-items-center"
                                    v-if="state == 'trialing'"
                                >
                                    <b-button
                                        type="is-success"
                                        size="is-large"
                                        @click="onSubscribeClick"
                                    >Subscribe</b-button>

                                    <span
                                        class="has-text-grey has-margin-left-1"
                                        v-if="customer.subscription.status == 'trialing'"
                                    >Trial ends on {{ customer.subscription.trialEnd | date }}</span>

                                    <b-button
                                        type="is-text"
                                        v-if="customer.subscription.status == 'active'"
                                    >Cancel my subscription</b-button>
                                </div>
                                <!-- Trialing First Payment -->
                                <div
                                    class="has-margin-bottom-3"
                                    v-if="state == 'trialing_will_upgrade'"
                                >
                                    <p class="is-size-5 has-text-weight-bold">Payment</p>
                                    <div>
                                        <div class="is-flex is-flex-row is-align-items-center">
                                            <p
                                                class="is-size-6"
                                                v-if="customer.subscription.nextPayment != null"
                                            >Your first bill for {{ (customer.subscription.price.amount / 100) | currency }} will be on {{ customer.subscription.nextPayment | date }}</p>
                                            <b-button
                                                class="has-padding-y-0 has-margin-y-0"
                                                type="is-text"
                                                @click="onCancel"
                                            >Cancel</b-button>
                                        </div>

                                        <p
                                            class="is-size-6"
                                        >{{ customer.paymentMethod.brand | uppercaseFirst }} ending in {{ customer.paymentMethod.last4 }}</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import billingStore from '../store/billing-store';
import { Subscription as SubscriptionObj, Customer } from '../../../api';
import { adminGuard } from '../../admin/router/admin-guard';
import moment from 'moment';
import { displayLoading } from '../../../core';

/**
 * User subscription page. Kinda hacked. Fix later.
 * Does not support more than 1 subscription plan.
 */
@Component
export default class Subscription extends Vue {
    // FUCK
    get name() {
        return billingStore.defaultPlan == null ? '' : billingStore.defaultPlan.name;
    }

    get monthPrice() {
        return billingStore.defaultPlan.prices.find(p => p.interval == 'month')!.amount / 100;
    }

    get yearPrice() {
        return billingStore.defaultPlan.prices.find(p => p.interval == 'year')!.amount / 100;
    }

    /**
     * The current state of the page and what should be displayed.
     */
    get state(): 'trialing' | 'trialing_will_upgrade' | 'active' | 'cancelling' | 'inactive' | 'issue' {
        switch (this.customer.subscription?.status) {
            case 'active':
                return this.customer.subscription.cancellingAtPeriodEnd ? 'cancelling' : 'active';
            case 'trialing':
                return this.customer.paymentMethod == null ? 'trialing' : 'trialing_will_upgrade';
            default:
                return 'inactive';
        }
    }

    showYearly = false;
    customer: Customer = null!;

    @displayLoading
    async created() {
        await billingStore.init();
        this.customer = billingStore.customer;
        this.showYearly = this.customer.subscription?.price.interval == 'year';
        console.log(this.state);
    }

    async onSubscribeClick() {
        if (this.customer.subscription?.status == 'trialing') {
            const price = billingStore.defaultPlan.prices.find(p => p.interval == (this.showYearly ? 'year' : 'month'));

            await billingStore.createCheckoutSession(price!.billingId);
        }
    }

    async onCancel() {
        if (this.customer.subscription != null && !this.customer.subscription.cancellingAtPeriodEnd) {
            await billingStore.cancelSubscriptionAtPeriodEnd();
        }
    }
}
</script>
