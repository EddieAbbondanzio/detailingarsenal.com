<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Subscription Plans"
                description="Plans for customers"
                icon="ferry"
                :backButtonTo="{name: 'adminPanel'}"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb
                            name="Subscription Plans"
                            :to="{name: 'subscriptionPlans'}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <b-button
                        @click="onRefreshClick"
                        type="is-success"
                        icon-left="refresh"
                        title="Pull in the subscription plans from Stripe"
                    >Refresh</b-button>
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="p in plans"
                :key="p.id"
                :title="p.name"
                :to="{name: 'subscriptionPlan', params: { id: p.id}}"
            ></list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import adminStore from '../../store/admin-store';
import { displayLoading } from '../../../../core';

@Component({
    name: 'subscription-plans'
})
export default class SubscriptionPlans extends Vue {
    get plans() {
        return adminStore.subscriptionPlans;
    }

    @displayLoading
    async created() {
        await adminStore.init();
    }

    @displayLoading
    async onRefreshClick() {
        await adminStore.refreshSubscriptionPlans();
    }
}
</script>