<template>
    <page>
        <template v-slot:header>
            <page-header title="Subscription" description="Subscription plan and billing">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Account" :to="{ name: 'account' }" />
                        <breadcrumb name="Subscription" :to="{ name: 'subscription' }" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="customer != null">
            <p class="is-size-4 has-text-weight-bold has-text-centered has-margin-bottom-3">One plan, no confusion.</p>

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
                                <!-- Trialing -->
                                <div class="is-flex is-flex-row is-align-items-center" v-if="state == 'trialing'">
                                    <b-button type="is-success" size="is-large" @click="onSubscribeClick"
                                        >Subscribe</b-button
                                    >

                                    <span
                                        class="has-text-grey has-margin-left-1"
                                        v-if="customer.subscription.status == 'trialing'"
                                        >Trial ends on {{ customer.subscription.trialPeriod.end | date }}</span
                                    >

                                    <b-button type="is-text" v-if="customer.subscription.status == 'active'"
                                        >Cancel my subscription</b-button
                                    >
                                </div>
                                <!-- Trialing First Payment -->
                                <div class="has-margin-bottom-3" v-else-if="state == 'trialing_will_upgrade'">
                                    <p class="is-size-5 has-text-weight-bold">Payment</p>
                                    <div>
                                        <div class="is-flex is-flex-row is-align-items-center">
                                            <p class="is-size-6">
                                                Your first bill for
                                                {{ (customer.subscription.price.amount / 100) | currency }} will be on
                                                {{ customer.subscription.period.end | date }}
                                            </p>
                                            <b-button
                                                class="has-padding-y-0 has-margin-y-0"
                                                type="is-text"
                                                @click="onCancel"
                                                title="Cancel my subscription"
                                                >Cancel</b-button
                                            >
                                        </div>

                                        <div class="is-flex is-flex-row is-align-items-center">
                                            <p
                                                v-for="paymentMethod in customer.paymentMethods"
                                                :key="paymentMethod.id"
                                                class="is-size-6"
                                            >
                                                {{ paymentMethod.brand | uppercaseFirst }} ending in
                                                {{ paymentMethod.last4 }}
                                            </p>
                                            <b-button
                                                class="has-padding-y-0 has-margin-y-0"
                                                type="is-text"
                                                @click="onAddCard"
                                                title="Update card on file"
                                                >Update</b-button
                                            >
                                        </div>
                                    </div>
                                </div>
                                <!-- Active -->
                                <div class="has-margin-bottom-3" v-else-if="state == 'active'">
                                    <p class="is-size-5 has-text-weight-bold">Payment</p>
                                    <div>
                                        <div class="is-flex is-flex-row is-align-items-center">
                                            <p class="is-size-6">
                                                Your next bill for
                                                {{ (customer.subscription.price.amount / 100) | currency }} will be on
                                                {{ customer.subscription.period.end | date }}
                                            </p>
                                            <b-button
                                                class="has-padding-y-0 has-margin-y-0"
                                                type="is-text"
                                                @click="onCancel"
                                                title="Cancel my subscription"
                                                >Cancel</b-button
                                            >
                                        </div>

                                        <div class="is-flex is-flex-row is-align-items-center">
                                            <p
                                                v-for="paymentMethod in customer.paymentMethods"
                                                :key="paymentMethod.id"
                                                class="is-size-6"
                                            >
                                                {{ paymentMethod.brand | uppercaseFirst }} ending in
                                                {{ paymentMethod.last4 }}
                                            </p>
                                            <b-button
                                                class="has-padding-y-0 has-margin-y-0"
                                                type="is-text"
                                                @click="onAddCard"
                                                title="Update card on file"
                                                >Update</b-button
                                            >
                                        </div>
                                    </div>
                                </div>
                                <!-- Issue -->
                                <div class="has-margin-bottom-3" v-else-if="state == 'issue'">
                                    <p class="is-size-5 has-text-weight-bold">Payment</p>
                                    <div>
                                        <div class="is-flex is-flex-row is-align-items-center">
                                            <b-icon icon="alert-circle" type="is-danger" class="has-margin-right-1" />
                                            <p class="has-text-weight-bold">
                                                Error. There was an issue processing your payment.
                                            </p>
                                        </div>

                                        <div class="is-flex is-flex-row is-align-items-center">
                                            <p class="is-size-6 has-margin-bottom-3">
                                                Your membership will expire on
                                                {{ customer.subscription.period.end | date }}
                                            </p>
                                        </div>

                                        <div class="is-flex is-flex-row is-align-items-center">
                                            <b-button
                                                class="has-padding-y-0 has-margin-y-0"
                                                type="is-primary"
                                                @click="onAddCard"
                                                title="Update card on file"
                                                >Update payment info</b-button
                                            >
                                        </div>
                                    </div>
                                </div>

                                <!-- Cancelling -->
                                <div v-else-if="state == 'cancelling'">
                                    <div class="is-flex is-flex-row is-align-items-center">
                                        <b-icon icon="alert" type="is-warning" class="has-margin-right-1" />
                                        <p class="has-text-weight-bold">Cancelling</p>
                                    </div>

                                    <div class="is-flex is-flex-row is-align-items-center">
                                        Your membership will end on {{ customer.subscription.period.end | date }}
                                        <b-button
                                            class="has-padding-y-0 has-margin-y-0"
                                            type="is-text"
                                            @click="onUndoCancel"
                                            title="Update card on file"
                                            >Undo</b-button
                                        >
                                    </div>
                                </div>
                                <!-- Inactive -->
                                <div class="is-flex is-flex-row is-align-items-center" v-if="state == 'inactive'">
                                    <b-button type="is-success" size="is-large" @click="onSubscribeClick"
                                        >Subscribe</b-button
                                    >
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
import { Subscription as SubscriptionObj, Customer } from '../../../../api';
import moment from 'moment';
import { displayLoading } from '../../../../core';
import billingStore from '@/modules/scheduling/subscription/store/billing-store';
import { loadStripeJs } from '@/plugins/stripe';

@Component
export default class Subscription extends Vue {
    // Work around
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
        if (this.customer.subscription == null) {
            return 'inactive';
        } else if (this.customer.subscription.cancellingAtPeriodEnd) {
            return 'cancelling';
        }

        switch (this.customer.subscription.status) {
            case 'active':
                return this.customer.subscription.cancellingAtPeriodEnd ? 'cancelling' : 'active';
            case 'trialing':
                return this.customer.paymentMethods.length == 0 ? 'trialing' : 'trialing_will_upgrade';
            case 'incomplete':
                return 'issue';
            case 'unpaid':
                return 'active';
            default:
                return 'inactive';
        }
    }

    showYearly = false;
    customer: Customer = null!;

    @displayLoading
    async created() {
        await loadStripeJs();
        await billingStore.init();
        this.customer = billingStore.customer;
        this.showYearly = this.customer.subscription?.price.interval == 'year';
    }

    @displayLoading
    async onSubscribeClick() {
        if (this.customer.subscription?.status == 'trialing') {
            const price = billingStore.defaultPlan.prices.find(p => p.interval == (this.showYearly ? 'year' : 'month'));

            await billingStore.createCheckoutSession(price!.billingId);
        }
    }

    @displayLoading
    async onCancel() {
        if (this.customer.subscription != null && !this.customer.subscription.cancellingAtPeriodEnd) {
            await billingStore.cancelSubscriptionAtPeriodEnd();
        }
    }

    @displayLoading
    async onUndoCancel() {
        if (this.customer.subscription != null && this.customer.subscription.cancellingAtPeriodEnd) {
            await billingStore.undoCancellingAtPeriodEnd();
        }
    }

    @displayLoading
    async onAddCard() {
        if (this.state != 'active' && this.state != 'trialing_will_upgrade') {
            return;
        }

        await billingStore.createCheckoutSession(this.customer.subscription!.price.billingId);
    }
}
</script>
