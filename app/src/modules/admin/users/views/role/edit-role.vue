<template>
    <page>
        <template v-slot:header>
            <page-header title="Edit role" :description="`Edit new role`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'usersPanel' }" />
                        <breadcrumb name="Roles" :to="{ name: 'roles' }" />
                        <breadcrumb :name="name" :to="{ name: 'role', params: { id: $route.params.id } }" />
                        <breadcrumb name="Edit" :to="{ name: 'editRole' }" active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes">
            <input-text-field label="Name" rules="required|max:32" :required="true" v-model="name" placeholder="user" />

            <input-group-header text="Permissions" />
            <b-table
                :data="permissions"
                checkable
                :checked-rows.sync="enabledPermissions"
                :custom-is-checked="
                    (a, b) => {
                        return a.id === b.id;
                    }
                "
            >
                <template>
                    <b-table-column v-slot="props" label="Permission" field="label" sortable>{{
                        props.row.label
                    }}</b-table-column>
                    <b-table-column v-slot="props" label="Action" field="action" sortable>{{
                        props.row.action
                    }}</b-table-column>
                    <b-table-column v-slot="props" label="Scope" field="scope" sortable>{{
                        props.row.scope
                    }}</b-table-column>
                </template>

                <template slot="empty">
                    <div class="is-flex is-justify-content-center">There's nothing here!</div>
                </template>
            </b-table>
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Permission, SpecificationError } from '@/api';
import { displayError, toast, displayLoading } from '@/core';
import securityStore from '../../store/security-store';

@Component({
    name: 'edit-role',
})
export default class EditRole extends Vue {
    get permissions() {
        return securityStore.permissions;
    }

    name = '';
    enabledPermissions: Permission[] = [];

    @displayLoading
    async created() {
        await securityStore.init();

        const role = securityStore.roles.find((r) => r.id == this.$route.params.id);

        this.name = role!.name;
        this.enabledPermissions = role?.permissions ?? [];
    }

    @displayLoading
    async onSubmit() {
        const edit = {
            id: this.$route.params.id,
            name: this.name,
            permissionIds: [...new Set(this.enabledPermissions.map((p) => p.id))],
        };

        try {
            const role = await securityStore.updateRole(edit);

            toast(`Updated role ${role.name}`);
            this.$router.push({ name: 'roles' });
        } catch (err) {
            if (err instanceof SpecificationError) {
                displayError(err);
            } else {
                throw err;
            }
        }
    }
}
</script>