<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Edit business"
                description="Edit the name, address, and contact info"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb name="Business" :to="{name: 'business'}" />
                        <breadcrumb name="Edit" :to="{name: 'editBusiness'}" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes">
            <input-text-field
                label="Name"
                placeholder="Jimbo's Buff Shop"
                rules="max:64"
                v-model="name"
            />

            <input-text-field
                label="Address"
                placeholder="123 Street Address, ST 13212"
                rules="max:128"
                v-model="address"
            />

            <input-text-field
                label="Phone"
                placeholder="555-123-1234"
                rules="max:32"
                v-model="phone"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import settingsStore from '../../store/settings-store';
import { UpdateBusiness } from '../../api/data-transfer-objects/update-business';
import { toast } from '@/core';
import { displayLoading } from '../../../../core/utils/display-loading';

@Component({
    name: 'edit-business'
})
export default class EditBusiness extends Vue {
    public name: string = '';
    public address: string = '';
    public phone: string = '';

    @displayLoading
    public async created() {
        await settingsStore.init();

        this.name = settingsStore.business.name || '';
        this.address = settingsStore.business.address || '';
        this.phone = settingsStore.business.phone || '';
    }

    @displayLoading
    public async onSubmit() {
        const update: UpdateBusiness = {
            id: settingsStore.business.id
        };

        if (this.name != '') {
            update.name = this.name;
        }

        if (this.address != '') {
            update.address = this.address;
        }

        if (this.phone != '') {
            update.phone = this.phone;
        }

        await settingsStore.updateBusiness(update);

        toast(`Edited business`);
        this.$router.push({ name: 'business' });
    }
}
</script>
