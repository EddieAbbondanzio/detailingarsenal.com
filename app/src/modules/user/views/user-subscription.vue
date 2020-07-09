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

        <div class="box is-shadowless">
            <p
                class="is-size-4 has-text-weight-bold has-text-centered has-margin-bottom-4"
            >One price, no surprises.</p>

            <div class="card has-margin-y-3">
                <!-- Status -->
                <div
                    v-if="isTrialing"
                    class="has-background-warning has-w-100 has-padding-all-1 has-text-centered"
                >Trialing</div>

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
                            <div class="is-flex is-flex-column">
                                <p class="is-size-1">
                                    <small class="is-size-3" style="vertical-align: text-top;">$</small>
                                    {{ price / 100 }}
                                </p>
                                <p
                                    class="is-size-5 has-text-grey is-uppercase"
                                    style="align-self: bottom"
                                >/ {{ interval }}</p>

                                <b-switch
                                    class="has-margin-y-3"
                                    type="is-info"
                                    v-model="showYearly"
                                >Yearly</b-switch>
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
                                    :outlined="isActive"
                                    :disabled="isActive"
                                >{{ isActive ? 'Active!' : 'Activate' }}</b-button>

                                <b-button type="is-text" v-if="isActive">Cancel my subscription</b-button>
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

@Component({})
export default class UserSubscription extends Vue {
    get name() {
        return billingStore.defaultPlan == null ? '' : billingStore.defaultPlan.info.name;
    }

    get isTrialing() {
        return false;
    }

    get isActive() {
        return false;
    }

    get price() {
        if (billingStore.defaultPlan == null) {
            return 0;
        }

        if (this.showYearly) {
            return billingStore.defaultPlan.prices.find(p => p.interval == 'year')!.amount;
        } else {
            return billingStore.defaultPlan.prices.find(p => p.interval == 'month')!.amount;
        }
    }

    get interval() {
        if (this.showYearly) {
            return 'year';
        } else {
            return 'month';
        }
    }

    async created() {
        await billingStore.init();
    }

    showYearly = false;
}
</script>