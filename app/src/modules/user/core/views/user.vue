<template>
    <page background="is-light">
        <template v-slot:header>
            <page-header
                title="User"
                description="User settings and info"
                icon="account"
                :backButton="false"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="User" :to="{name: 'user'}" :active="true" />
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
export default class UserView extends Vue {
    entries: { title: string; description: string; icon: string; to: RawLocation }[] = [
        {
            title: 'Profile',
            description: 'Username and more info',
            icon: 'account',
            to: { name: 'profile' }
        }
    ];
}
</script>