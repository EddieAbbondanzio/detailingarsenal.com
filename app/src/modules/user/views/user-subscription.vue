<template>
    <page>
        <template v-slot:header>
            <page-header title="Subscription" description="Subscription plan and billing">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Subscription" :to="{ name: 'userSubscription' }" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="customer != null">
            <p class="is-size-4 has-text-weight-bold has-text-centered has-margin-bottom-3">One plan, no confusion.</p>

            <div class="card has-margin-top-3 has-margin-bottom-4">
                <!-- Status -->
                <div
                    v-if="customer.subscription.status == 'trialing'"
                    class="has-background-warning has-w-100 has-padding-all-1 has-text-centered"
                >
                    Trialing. {{ customer.subscription.trialDaysRemaining }}
                    {{ customer.subscription.trialDaysRemaining == 1 ? 'day' : 'days' }} remaining.
                </div>

                <!-- Title -->
                <div class="has-background-primary has-padding-all-4">
                    <p class="is-size-4 has-text-weight-bold has-text-white has-text-centered">
                        Detailing Arsenal {{ name }}
                    </p>
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
                                <p class="is-size-5 has-text-grey is-uppercase" style="align-self: bottom">
                                    / {{ showYearly ? 'year' : 'month' }}
                                </p>

                                <b-switch class="has-margin-y-3" type="is-info" v-model="showYearly">
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
                                    <b-icon icon="check" type="is-success" class="has-padding-right-3" />Unlimited
                                    appointment bookings
                                </li>
                                <li>
                                    <b-icon icon="check" type="is-success" class="has-padding-right-3" />Unlimited
                                    contacts
                                </li>
                                <li>
                                    <b-icon icon="check" type="is-success" class="has-padding-right-3" />Synchronized
                                    multi device support
                                </li>
                            </ul>

                            <div class="is-flex is-flex-row is-align-items-center">
                                <b-button
                                    type="is-success"
                                    size="is-large"
                                    :outlined="customer.subscription.status == 'active'"
                                    :disabled="customer.subscription.status == 'active'"
                                    @click="onActionClick"
                                    >{{ customer.subscription.status == 'active' ? 'Active!' : 'Activate' }}</b-button
                                >

                                <span
                                    class="has-text-grey has-margin-left-1"
                                    v-if="customer.subscription.status == 'trialing'"
                                    >Trial ends on {{ customer.subscription.trialEnd | date }}</span
                                >

                                <b-button type="is-text" v-if="customer.subscription.status == 'active'"
                                    >Cancel my subscription</b-button
                                >
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="has-margin-y-3">
                <p class="is-size-4 has-text-weight-bold has-margin-bottom-3">Your Subscription</p>
                <div class="has-margin-bottom-3">
                    <p class="is-size-4 has-text-weight-bold">Plan</p>
                    <p class="is-size-6">{{ customer.subscription.planName }}</p>
                </div>

                <div class="has-margin-bottom-3">
                    <p class="is-size-4 has-text-weight-bold">Status</p>
                    <p class="is-size-6 has-text-success">{{ customer.subscription.status }}</p>
                </div>

                <div class="has-margin-bottom-3" v-if="customer.paymentMethod != null">
                    <p class="is-size-4 has-text-weight-bold">Payment Method</p>
                    <p class="is-size-6">
                        {{ customer.paymentMethod.brand }} ending in {{ customer.paymentMethod.last4 }}
                    </p>
                    <p class="is-size-6 has-text-grey">
                        Next payment on {{ customer.subscription.nextPayment | date }}
                    </p>
                    <b-button type="is-text">Update</b-button>
                </div>
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import billingStore from '../store/billing-store';
import { Subscription, Customer } from '../../../api';
import { adminGuard } from '../../admin/router/admin-guard';
import moment from 'moment';
import { displayLoading } from '../../../core';

/**
 * User subscription page. Kinda hacked. Fix later.
 * Does not support more than 1 subscription plan.
 */
@Component({})
export default class UserSubscription extends Vue {
    get name() {
        return billingStore.defaultPlan == null ? '' : billingStore.defaultPlan.name;
    }

    get monthPrice() {
        return billingStore.defaultPlan.prices.find(p => p.interval == 'month')!.amount / 100;
    }

    get yearPrice() {
        return billingStore.defaultPlan.prices.find(p => p.interval == 'year')!.amount / 100;
    }

    showYearly = false;
    customer: Customer = null!;

    @displayLoading
    async created() {
        await billingStore.init();
        this.customer = billingStore.customer;
        this.showYearly = this.customer.subscription.price.interval == 'year';
    }

    async onActionClick() {
        if (this.customer.subscription.status == 'trialing') {
            const price = billingStore.defaultPlan.prices.find(p => p.interval == (this.showYearly ? 'year' : 'month'));

            await billingStore.createCheckoutSession(price!.billingId);
        }
    }
}
</script>
