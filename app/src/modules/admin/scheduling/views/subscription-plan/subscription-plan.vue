<template>
    <page>
        <template v-slot:header>
            <page-header :title="plan != null ? plan.name : ``">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Scheduling panel" :to="{ name: 'schedulingPanel' }" />
                        <breadcrumb name="Subscription plans" :to="{ name: 'subscriptionPlans' }" />
                        <breadcrumb
                            :name="plan != null ? plan.name : ''"
                            :to="{ name: 'subscriptionPlan', params: $route.params }"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <update-button :to="{ name: 'editSubscriptionPlan', params: { id: $route.params.id } }" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="plan != null">
            <div class="has-margin-bottom-4">
                <h5 class="is-size-4 title has-margin-bottom-2">{{ plan.name }}</h5>
            </div>

            <p v-if="plan.description">{{ plan.description }}</p>

            <b-field label="Prices">
                <b-table :data="plan.prices">
                    <template>
                        <b-table-column v-slot="props" label="Price" field="label" sortable>
                            {{ (props.row.amount / 100) | currency }}
                        </b-table-column>
                        <b-table-column v-slot="props" label="Interval" field="action" sortable>{{
                            props.row.interval
                        }}</b-table-column>
                    </template>

                    <template slot="empty">
                        <div class="is-flex is-justify-content-center">There's nothing here!</div>
                    </template>
                </b-table></b-field
            >
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { SubscriptionPlan } from '@/api/scheduling';
import { displayLoading } from '@/core';
import subscriptionPlanStore from '../../store/subscription-plan-store';

@Component({})
export default class SubscriptionPlanView extends Vue {
    plan: SubscriptionPlan = null!;

    @displayLoading
    async created() {
        await subscriptionPlanStore.init();
        this.plan = subscriptionPlanStore.subscriptionPlans.find(p => p.id == this.$route.params.id)!;
    }
}
</script>
