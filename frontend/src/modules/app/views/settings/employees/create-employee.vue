<template>
    <action-page
        title="Create new vehicle category"
        description="A new vehicle category to organize price and time durations"
        :breadcrumbs="[
                { name: 'Settings', to: { name: 'settings' } },
                { name: 'Employees', to: { name: 'employees' } },
                { name: 'Create', to: { name: 'createEmployee' } }
            ]"
        actionText="Create"
        @input="onSubmit"
        :loading="loading"
    >
        <b-field class="is-required" label="Name">
            <!-- Name -->
            <validation-provider name="Name" rules="required|max:64" v-slot="{ errors, classes }">
                <b-input type="text" v-model="name" :class="classes" placeholder="Dwight Shrute"></b-input>
                <error-message :text="errors[0]" />
            </validation-provider>
        </b-field>

        <b-field label="Position">
            <validation-provider name="Position" rules="max:32" v-slot="{ errors, classes }">
                <b-input
                    v-model="position"
                    :class="classes"
                    placeholder="Assistant Regional Manager"
                ></b-input>
                <error-message :text="errors[0]" />
            </validation-provider>
        </b-field>
    </action-page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ValidationProvider } from 'vee-validate';
import { ValidationObserver } from 'vee-validate';
import { getModule } from 'vuex-module-decorators';
import SettingsStore from '../../../store/settings/settings-store';
import { toast } from '../../../../../core';

/**
 * View to create a new employee.
 */
@Component({
    name: 'create-vehicle-category',
    components: {
        ValidationProvider,
        ValidationObserver
    }
})
export default class CreateEmployee extends Vue {
    public name: string = '';
    public position: string = '';
    public loading: boolean = false;

    public async onSubmit() {
        this.loading = true;
        const employee = { name: this.name, position: this.position };

        const settingsStore = getModule(SettingsStore, this.$store);

        await settingsStore.createEmployee(employee);

        toast(`Created new employee ${employee.name}`);

        this.$router.push({ name: 'employees' });
        this.loading = false;
    }
}
</script>
