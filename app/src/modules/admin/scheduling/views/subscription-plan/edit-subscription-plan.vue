<template>
    <page>
        <template v-slot:header>
            <page-header title="Edit subscription plan" :description="`Edit name, description, and role`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Scheduling panel" :to="{ name: 'schedulingPanel' }" />
                        <breadcrumb name="Subscription plans" :to="{ name: 'subscriptionPlans' }" />
                        <breadcrumb :name="name" :to="{ name: 'subscriptionPlan', params: { id: $route.params.id } }" />
                        <breadcrumb name="Edit" :to="{ name: 'editSubscriptionPlan' }" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes">
            <input-text-field label="Name" v-model="name" placeholder="Plan name" :disabled="true" />

            <input-text-field
                type="textarea"
                rules="max:1024"
                maxLength="1024"
                label="Description"
                v-model="description"
                placeholder="Some longer text giving more details."
            />

            <input-select v-model="roleId">
                <option :value="null">No role</option>
                <option :value="r.id" v-for="r in roles" :key="r.id">{{ r.name }}</option>
            </input-select>
        </input-form>
    </page>
</template>

<script lang="ts">
import { SpecificationError } from '@/api/shared';
import { displayError, displayLoading, toast } from '@/core';
import securityStore from '@/modules/admin/users/store/security-store';
import Vue from 'vue';
import Component from 'vue-class-component';
import subscriptionPlanStore from '../../store/subscription-plan-store';

@Component
export default class EditSubscriptionPlan extends Vue {
    get roles() {
        return securityStore.roles;
    }

    name: string = '';
    description: string | null = null;
    roleId: string | null = null;

    @displayLoading
    async created() {
        await securityStore.init();
        await subscriptionPlanStore.init();

        const plan = subscriptionPlanStore.subscriptionPlans.find(p => p.id == this.$route.params.id)!;

        this.name = plan.name;
        this.description = plan.description;
        this.roleId = plan.roleId;
    }

    @displayLoading
    async onSubmit() {
        const edit = {
            id: this.$route.params.id,
            description: this.description,
            roleId: this.roleId
        };

        try {
            const plan = await subscriptionPlanStore.updateSubscriptionPlan(edit)!;

            toast(`Updated subscription plan ${plan.name}`);
            this.$router.push({ name: 'subscriptionPlans' });
        } catch (err) {
            if (err instanceof SpecificationError) {
                displayError(err);
            } else {
                throw err;
            }
        }
    }
}
</script>
