<template>
    <page :loading="loading">
        <template v-slot:header>
            <page-header
                title="Services"
                :description="`${count} ${count == 1 ? 'service' : 'services'}`"
                icon="toolbox"
                :backButtonTo="{name: 'settings'}"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb name="Services" :to="{name: 'services'}" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{name: 'createService' }" text="Create service" />
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="s in services"
                :key="s.id"
                :title="s.name"
                :description="s.description"
                :to="{name: 'service', params: {id: s.id}}"
                height="124px"
            >
                <template v-slot:actions>
                    <edit-delete-dropdown @edit="onEdit(s)" @delete="onDelete(s)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Service } from '@/modules/app/api';
import { confirmDelete } from '@/modules/app/utils/confirm-delete/confirm-delete';
import { toast } from '@/core';
import { displayError } from '../../../utils/display-error/display-error';
import settingsStore from '../../../store/settings/settings-store';

@Component({
    name: 'services'
})
export default class Services extends Vue {
    public loading: boolean = true;

    get count() {
        return settingsStore.services.length;
    }

    get services() {
        return settingsStore.services;
    }

    public async created() {
        await settingsStore.init();

        this.loading = false;
    }

    async onEdit(s: Service) {
        this.$router.push({ name: 'editService', params: { id: s.id } });
    }

    async onDelete(s: Service) {
        const del = await confirmDelete('vehicle category', s.name);

        if (del) {
            try {
                this.loading = true;
                await settingsStore.deleteService(s);

                toast(`Delete vehicle category ${s.name}`);
            } catch (err) {
                displayError(err);
            } finally {
                this.loading = false;
            }
        }
    }
}
</script>
