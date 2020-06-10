<template>
    <page>
        <template v-slot:header>
            <page-header :title="plan != null ? plan.name : ``">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Subscription Plans" :to="{name: 'subscriptionPlans'}" />
                        <breadcrumb
                            :name="plan != null ? plan.name : ''"
                            :to="{name: 'subscriptionPlan', params: $route.params}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="plan != null">
            <div class="has-margin-bottom-4">
                <h5 class="is-size-4 title has-margin-bottom-2">{{ plan.name }}</h5>
            </div>

            <input-group-header text="Prices" />

            <b-table :data="plan.prices">
                <template slot-scope="props">
                    <b-table-column
                        label="Price"
                        field="label"
                        sortable
                    >{{ (props.row.price / 100) | currency }}</b-table-column>
                    <b-table-column
                        label="Interval"
                        field="action"
                        sortable
                    >{{ props.row.interval }}</b-table-column>
                </template>

                <template slot="empty">
                    <div class="is-flex is-justify-content-center">There's nothing here!</div>
                </template>
            </b-table>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import adminStore from '../../store/admin-store';
import { displayLoading } from '../../../../core';
import { SubscriptionPlan } from '../../api/entities/subscription-plan';

@Component({})
export default class SubscriptionPlanView extends Vue {
    plan: SubscriptionPlan = null!;

    @displayLoading
    async created() {
        await adminStore.init();
        this.plan = adminStore.subscriptionPlans.find(p => p.id == this.$route.params.id)!;
    }
}
</script>