<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Edit subscription plan"
                :description="`Edit subscription plan ${name}`"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Subscription Plans" :to="{name: 'subscriptionPlans'}" />
                        <breadcrumb
                            :name="name"
                            :to="{name: 'subscriptionPlan', params: $route.params}"
                        />
                        <breadcrumb name="Edit" :to="{name: 'editSubscriptionPlan'}" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes">
            <input-text-field
                label="Name"
                rules="required|max:32"
                :required="true"
                v-model="name"
                placeholder="user"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import adminStore from '../../store/admin-store';
import { displayLoading, toast } from '../../../../core';

@Component({})
export default class EditSubscriptionPlan extends Vue {
    name: string = '';
    roleId: string | null = null;

    @displayLoading
    async created() {
        await adminStore.init();

        const plan = adminStore.subscriptionPlans.find(p => p.id == this.$route.params.id)!;

        this.name = plan.name;
        this.roleId = plan.roleId;
    }

    @displayLoading
    async onSubmit() {
        const edit = {
            id: this.$route.params.id,
            roleId: this.roleId
        };

        const role = await adminStore.updateSubscriptionPlan(edit);

        toast(`Updated subscription plan ${this.name}`);
        this.$router.push({ name: 'subscriptionPlans' });
    }
}
</script>