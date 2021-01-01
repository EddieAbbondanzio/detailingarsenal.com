<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Subscription plans"
                description="Plans for customers"
                icon="ferry"
                :backButtonTo="{ name: 'adminPanel' }"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Scheduling panel" :to="{ name: 'schedulingPanel' }" />
                        <breadcrumb name="Subscription plans" :to="{ name: 'subscriptionPlans' }" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <b-button
                        @click="onRefreshClick"
                        type="is-success"
                        icon-left="refresh"
                        title="Pull in the subscription plans from Stripe"
                        >Refresh</b-button
                    >
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="p in plans"
                :key="p.id"
                :title="p.name"
                :to="{ name: 'subscriptionPlan', params: { id: p.id } }"
            >
                <template v-slot:actions>
                    <update-delete-dropdown @edit="onEdit(p)" @delete="onDelete(p)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { displayLoading } from '@/core';
import subscriptionPlanStore from '@/modules/admin/scheduling/store/subscription-plan-store';
import { SubscriptionPlan } from '@/api';

@Component({
    name: 'subscription-plans',
})
export default class SubscriptionPlans extends Vue {
    get plans() {
        return subscriptionPlanStore.subscriptionPlans;
    }

    @displayLoading
    async created() {
        await subscriptionPlanStore.init();
    }

    @displayLoading
    async onRefreshClick() {
        await subscriptionPlanStore.refreshSubscriptionPlans();
    }

    onEdit(p: SubscriptionPlan) {
        this.$router.push({ name: 'editSubscriptionPlan', params: { id: p.id } });
    }

    onDelete(p: SubscriptionPlan) {
        alert('Plans can only be deleted via Stripe.');
    }
}
</script>