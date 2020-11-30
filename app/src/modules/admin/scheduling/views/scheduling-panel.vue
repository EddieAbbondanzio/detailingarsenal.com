<template>
    <page background="is-light">
        <template v-slot:header>
            <page-header
                title="Scheduling panel"
                description="Scheduling specific controls"
                icon="cogs"
                :backButton="false"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Scheduling panel" :to="{ name: 'schedulingPanel' }" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="setting in entries"
                :key="setting.title"
                :title="setting.title"
                :description="setting.description"
                :to="setting.to"
            >
                <template v-slot:icon>
                    <b-icon :icon="setting.icon" size="is-medium" type="is-primary" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

@Component({})
export default class SchedulingPanel extends Vue {
    entries: { title: string; description: string; icon: string; to: RawLocation }[] = [
        {
            title: 'Subscription plans',
            description: 'Plans for customers',
            icon: 'ferry',
            to: { name: 'subscriptionPlans' },
        },
    ];
}
</script>