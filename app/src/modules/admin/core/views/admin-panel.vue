<template>
    <page background="is-light">
        <template v-slot:header>
            <page-header title="Admin panel" description="Top secret stuff" icon="cogs" :backButton="false">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" :active="true" />
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

@Component({
    name: 'admin-panel'
})
export default class AdminPanel extends Vue {
    entries: { title: string; description: string; icon: string; to: RawLocation }[] = [
        {
            title: 'Scheduling',
            description: 'Scheduling specific controls',
            icon: 'calendar',
            to: { name: 'schedulingPanel' }
        },
        {
            title: 'Product catalog',
            description: 'Product catalog management',
            icon: 'book-open',
            to: { name: 'productCatalogPanel' }
        },
        {
            title: 'Users',
            description: 'Users of the site',
            icon: 'account',
            to: { name: 'usersPanel' }
        }
    ];
}
</script>
