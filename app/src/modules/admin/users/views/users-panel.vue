<template>
    <page background="is-light">
        <template v-slot:header>
            <page-header title="Users Panel" description="Users specific controls" icon="cogs" :backButton="false">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'usersPanel' }" :active="true" />
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
export default class UsersPanel extends Vue {
    entries: { title: string; description: string; icon: string; to: RawLocation }[] = [
        {
            title: 'Permissions',
            description: 'Access control',
            icon: 'lock',
            to: { name: 'permissions' },
        },
        {
            title: 'Roles',
            description: 'Collection of roles',
            icon: 'account',
            to: { name: 'roles' },
        },
    ];
}
</script>