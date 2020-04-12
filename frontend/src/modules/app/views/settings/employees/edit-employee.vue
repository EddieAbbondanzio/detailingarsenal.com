<template>
    <action-page
        title="Edit employee"
        description="Edit an existing employee"
        :breadcrumbs="[
                { name: 'Settings', to: { name: 'settings' } },
                { name: 'Employees', to: { name: 'employees' } },
                { name: 'Edit', to: { name: 'editEmployee', params: $route.params } }
            ]"
        actionText="Save changes"
        @input="onSubmit"
        :loading="loading"
    >
        <b-field class="is-required" label="Name">
            <!-- Name -->
            <validation-provider name="Name" rules="required|max:32" v-slot="{ errors, classes }">
                <b-input
                    type="text"
                    v-model="name"
                    :class="classes"
                    :disabled="loading"
                    placeholder="Mid-size SUV"
                ></b-input>
                <error-message :text="errors[0]" />
            </validation-provider>
        </b-field>

        <b-field label="Description">
            <validation-provider name="Position" rules="max:32" v-slot="{ errors, classes }">
                <b-input
                    v-model="position"
                    :class="classes"
                    :disabled="loading"
                    placeholder="Mid sized SUVs with 2 rows of seats."
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
 * View to edit an employee.
 */
@Component({
    name: 'edit-employee',
    components: {
        ValidationObserver,
        ValidationProvider
    }
})
export default class EditEmployee extends Vue {
    public name: string = '';
    public position: string = '';
    public loading: boolean = true;

    public async created() {
        const id = this.$route.params.id;
        const settingsStore = getModule(SettingsStore, this.$store);
        await settingsStore.init();
        const { name, position } = settingsStore.employees.find(e => e.id == id)!;

        this.name = name;
        this.position = position;
        this.loading = false;
    }

    public async onSubmit() {
        this.loading = true;
        const settingsStore = getModule(SettingsStore, this.$store);
        await settingsStore.updateEmployee({
            name: this.name,
            position: this.position,
            id: this.$route.params.id
        });

        toast(`Edited employee ${this.name}`);
        this.$router.push({ name: 'employees' });
        this.loading = false;
    }
}
</script>
