<template>
    <content-page
        title="Employees"
        description="Manage employees that can perform services "
        :breadcrumbs="[
                { name: 'Settings', to: { name: 'settings' } },
                { name: 'Employees', to: { name: 'employees' } }
            ]"
        :loading="loading"
    >
        <template v-slot:action>
            <b-button
                type="is-success"
                icon-left="plus-circle-outline"
                tag="router-link"
                :to="{ name: 'createEmployee' }"
            >Create employee</b-button>
        </template>

        <span class="has-text-grey has-text-centered" v-if="employees.length == 0">No employees</span>
        <employee-card v-for="employee in employees" :value="employee" :key="employee.id" />
    </content-page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import EmployeeCard from '@/modules/app/components/settings/employee-card.vue';
import SettingsStore from '@/modules/app/store/settings/settings-store';

/**
 * View to display the employees.
 */
@Component({
    name: 'employees',
    components: {
        EmployeeCard
    }
})
export default class Employees extends Vue {
    public loading: boolean = true;

    get employees() {
        const settingsStore = getModule(SettingsStore, this.$store);
        return settingsStore.employees;
    }

    public async created() {
        const settingsStore = getModule(SettingsStore, this.$store);
        await settingsStore.init();
        this.loading = false;
    }
}
</script>