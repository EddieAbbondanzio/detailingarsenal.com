<template>
    <page :loading="loading">
        <template v-slot:header>
            <page-header
                title="Vehicle categories"
                :description="`${count} vehicle ${count == 1 ? 'category' : 'categories'}`"
                icon="car"
                :backButtonTo="{name: 'settings'}"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb
                            name="Vehicle categories"
                            :to="{name: 'vehicleCategories'}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button
                        :to="{name: 'createVehicleCategory' }"
                        text="Create vehicle category"
                    />
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="vc in vehicleCategories"
                :key="vc.id"
                :title="vc.name"
                :description="vc.description"
                height="124px"
            >
                <template v-slot:actions>
                    <edit-delete-dropdown @edit="onEdit(vc)" @delete="onDelete(vc)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import { toast } from '@/core';
import { VehicleCategory } from '../../api/entities/vehicle-category';
import settingsStore from '../../store/settings-store';
import { confirmDelete } from '@/core/utils/confirm-delete/confirm-delete';
import { displayError } from '@/core/utils/display-error/display-error';

/**
 * Settings page for managing vehicle categories.
 */
@Component({
    name: 'vehicle-categories'
})
export default class VehicleCategories extends Vue {
    loading: boolean = true;

    get count() {
        return settingsStore.vehicleCategories.length;
    }

    get vehicleCategories() {
        return settingsStore.vehicleCategories;
    }

    async created() {
        await settingsStore.init();
        this.loading = false;
    }

    async onEdit(vc: VehicleCategory) {
        this.$router.push({ name: 'editVehicleCategory', params: { id: vc.id } });
    }

    async onDelete(vc: VehicleCategory) {
        const del = await confirmDelete('vehicle category', vc.name);

        if (del) {
            try {
                this.loading = true;
                await settingsStore.deleteVehicleCategory(vc);

                toast(`Delete vehicle category ${vc.name}`);
            } catch (err) {
                displayError(err);
            } finally {
                this.loading = false;
            }
        }
    }
}
</script>
