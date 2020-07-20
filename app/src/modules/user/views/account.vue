<template>
    <page background="is-light">
        <template v-slot:header>
            <page-header
                title="My Account"
                description="Account details and settings"
                icon="account"
                :backButton="false"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Account" :to="{ name: 'user' }" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="entry in entries"
                :key="entry.title"
                :title="entry.title"
                :description="entry.description"
                :to="entry.to"
            >
                <template v-slot:icon>
                    <b-icon :icon="entry.icon" size="is-medium" type="is-primary" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';
import userStore from '../store/user-store';

@Component({
    name: 'user'
})
export default class User extends Vue {
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
}
</script>
