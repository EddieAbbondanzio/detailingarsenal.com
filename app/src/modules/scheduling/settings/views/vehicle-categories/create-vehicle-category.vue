<template>
    <page>
        <template v-slot:header>
            <page-header title="Create vehicle category" :description="`Create vehicle category`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{ name: 'settings' }" />
                        <breadcrumb name="Vehicle categories" :to="{ name: 'vehicleCategories' }" />
                        <breadcrumb name="Create" :to="{ name: 'createVehicleCategory' }" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Create">
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
import { getModule } from 'vuex-module-decorators';
import ErrorMessage from '@/components/common/input/error-message.vue';
import ActionPage from '@/components/common/pages/action-page.vue';
import InputTextField from '@/core/components/input/input-text-field.vue';
import { toast, displayLoading, displayError } from '@/core';
import settingsStore from '../../store/settings-store';
import { ValidationError, SpecificationError } from '@/api/shared';

/**
 * View to create a new vehicle category.
 */
@Component({
    name: 'create-vehicle-category'
})
export default class CreateVehicleCategory extends Vue {
    @Ref('nameField')
    public nameField!: InputTextField;

    public name: string = '';
    public description: string = '';

    @displayLoading
    public async onSubmit() {
        const create = { name: this.name, description: this.description };

        try {
            await settingsStore.createVehicleCategory(create);

            toast(`Created new vehicle category ${create.name}`);
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
