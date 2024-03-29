<template>
    <page background="is-light">
        <template v-slot:header>
            <page-header
                title="Settings"
                description="Manage business settings"
                icon="cogs"
                :backButton="false"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" :active="true" />
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
import settingsStore from '../store/settings-store';

@Component({
    name: 'settings'
})
export default class Settings extends Vue {
    entries: { title: string; description: string; icon: string; to: RawLocation }[] = [
        {
            title: 'Business',
            description: 'Name, address, and contact info',
            icon: 'domain',
            to: { name: 'business' }
        },
        {
            title: 'Hours Of Operation',
            description: 'Hours the business is open',
            icon: 'clock-outline',
            to: { name: 'hoursOfOperation' }
        },
        {
            title: 'Vehicle Categories',
            description: 'Vehicle categories for service prices and durations',
            icon: 'car',
            to: { name: 'vehicleCategories' }
        },
        {
            title: 'Services',
            description: 'Services that the business offers',
            icon: 'toolbox',
            to: { name: 'services' }
        }
    ];

    public async created() {
        await settingsStore.init();
    }
}
</script>
