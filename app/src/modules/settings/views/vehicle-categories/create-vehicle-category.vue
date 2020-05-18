<template>
    <page :loading="loading">
        <template v-slot:header>
            <page-header
                title="Edit vehicle category"
                :description="`Edit vehicle category ${name}`"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb name="Vehicle categories" :to="{name: 'vehicleCategories'}" />
                        <breadcrumb
                            name="Create"
                            :to="{name: 'createVehicleCategory'}"
                            active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" :loading="loading" submitText="Create">
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
import { ValidationError, SpecificationError, toast } from '@/core';
import settingsStore from '../../store/settings-store';
import { displayError } from '@/core/utils/display-error/display-error';

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
    public loading: boolean = false;

    public async onSubmit() {
        this.loading = true;
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
        } finally {
            this.loading = false;
        }
    }
}
</script>
