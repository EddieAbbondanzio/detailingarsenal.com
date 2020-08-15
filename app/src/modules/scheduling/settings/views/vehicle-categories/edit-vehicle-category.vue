<template>
    <page>
        <template v-slot:header>
            <page-header :title="`Edit ${name}`" description="Edit an existing vehicle category">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb name="Vehicle categories" :to="{name: 'vehicleCategories'}" />
                        <breadcrumb
                            name="Edit"
                            :to="{name: 'editVehicleCategory', params: $route.params}"
                            active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes">
            <input-text-field
                ref="nameField"
                label="Name"
                rules="required|max:32"
                :required="true"
                v-model="name"
                placeholder="Mid-sized SUV"
            />

            <input-text-field
                ref="descriptionField"
                label="Description"
                rules="max:128"
                v-model="description"
                placeholder="Mid-sized SUV with 2 rows of seats"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import { toast, displayLoading, displayError } from '@/core';
import InputTextField from '@/core/components/input/input-text-field.vue';
import settingsStore from '../../store/settings-store';
import { ValidationError, SpecificationError } from '@/api';

/**
 * View to edit a vehicle category.
 */
@Component({
    name: 'edit-vehicle-category'
})
export default class EditVehicleCategory extends Vue {
    name: string = '';
    description: string = '';

    @displayLoading
    async created() {
        const id = this.$route.params.id;
        await settingsStore.init();

        const vc = settingsStore.vehicleCategories.find(vc => vc.id == id);

        if (vc == null) {
            throw new Error(`Vehicle category with id ${id} does not exist.`);
        }

        this.name = vc.name;
        this.description = vc.description;
    }

    async mounted() {}

    @displayLoading
    async onSubmit() {
        const update = {
            id: this.$route.params.id,
            name: this.name,
            description: this.description
        };

        try {
            await settingsStore.updateVehicleCategory(update);

            toast(`Edited vehicle category ${update.name}`);
            this.$router.push({ name: 'vehicleCategories' });
        } catch (err) {
            if (err instanceof ValidationError) {
                const nameError = err.getErrorByField('name');
                const descriptionError = err.getErrorByField('description');

                if (nameError != null) {
                    (this.$refs.nameField as InputTextField).setError(nameError.message);
                }

                if (descriptionError != null) {
                    (this.$refs.descriptionField as InputTextField).setError(descriptionError.message);
                }
            } else if (err instanceof SpecificationError) {
                displayError(err);
            } else {
                throw err;
            }
        }
    }
}
</script>
