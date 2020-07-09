<template>
    <page>
        <template v-slot:header>
            <page-header title="Subscription" description="Subscription plan and billing">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb
                            name="Subscription"
                            :to="{name: 'userSubscription'}"
                            active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="subscription != null">
            <p
                class="is-size-4 has-text-weight-bold has-text-centered has-margin-bottom-4"
            >One plan, no confusion.</p>

            <div class="card has-margin-y-3">
                <!-- Status -->
                <div
                    v-if="subscription.status == 'trialing'"
                    class="has-background-warning has-w-100 has-padding-all-1 has-text-centered"
                >Trialing. {{ trialDaysRemaining }} {{ trialDaysRemaining == 1 ? 'day' : 'days' }} remaining.</div>

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
                                    />Unlimited appointment bookings
                                </li>
                                <li>
                                    <b-icon
                                        icon="check"
                                        type="is-success"
                                        class="has-padding-right-3"
                                    />Unlimited contacts
                                </li>
                                <li>
                                    <b-icon
                                        icon="check"
                                        type="is-success"
                                        class="has-padding-right-3"
                                    />Synchronized multi device support
                                </li>
                            </ul>

                            <div class="is-flex is-flex-row is-align-items-center">
                                <b-button
                                    type="is-success"
                                    size="is-large"
                                    :outlined="subscription.status == 'active'"
                                    :disabled="subscription.status == 'active'"
                                    @click="onActionClick"
                                >{{ subscription.status == 'active' ? 'Active!' : 'Activate' }}</b-button>

                                <b-button
                                    type="is-text"
                                    v-if="subscription.status == 'active'"
                                >Cancel my subscription</b-button>
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
import { Subscription } from '../../../api';
import { adminGuard } from '../../admin/router/admin-guard';
import moment from 'moment';

/**
 * User subscription page. Kinda hacked. Fix later.
 * Does not support more than 1 subscription plan.
 */
@Component({})
export default class UserSubscription extends Vue {
    get name() {
        return billingStore.defaultPlan == null ? '' : billingStore.defaultPlan.info.name;
    }

    get monthPrice() {
        return billingStore.defaultPlan.prices.find(p => p.interval == 'month')!.amount / 100;
    }

    get yearPrice() {
        return billingStore.defaultPlan.prices.find(p => p.interval == 'year')!.amount / 100;
    }

    get trialDaysRemaining() {
        const today = moment();
        const trialEnd = moment(this.subscription.trialEnd);

        return trialEnd.diff(today, 'days');
    }

    subscription: Subscription = null!;
    showYearly = false;

    async created() {
        await billingStore.init();
        this.subscription = billingStore.subscription;
        this.showYearly = this.subscription.price.interval == 'year';
    }

    async onActionClick() {
        if (this.subscription.status == 'trialing') {
            const price = billingStore.defaultPlan.prices.find(p => p.interval == (this.showYearly ? 'year' : 'month'));

            await billingStore.startCheckout(price!.billingId);
        }
    }
}
</script>