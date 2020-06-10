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
                    <create-button
                        @click="onRefreshClick"
                        text="Refresh"
                        title="Pull in the subscription plans from Stripe"
                    />
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="p in plans"
                :key="p.id"
                :title="p.name"
                :to="{name: 'subscriptionPlan', params: { id: p.id}}"
            >
                <template v-slot:actions>
                    <edit-delete-dropdown @edit="onEdit(r)" @delete="onDelete(r)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import adminStore from '../../store/admin-store';

@Component({
    name: 'subscription-plans'
})
export default class SubscriptionPlans extends Vue {
    get plans() {
        return adminStore.subscriptionPlans;
    }

    async onRefreshClick() {
        await adminStore.refreshSubscriptionPlans();
    }
}
</script>