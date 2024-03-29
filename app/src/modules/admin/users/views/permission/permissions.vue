<template>
    <page>
        <template v-slot:header>
            <page-header title="Permissions" description="Access control" icon="lock">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'schedulingPanel' }" />
                        <breadcrumb name="Permissions" :to="{ name: 'permissions' }" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{ name: 'createPermission' }" text="Create permission" />
                </template>
            </page-header>
        </template>

        <b-table :data="permissions">
            <b-table-column v-slot="props" label="Permission" field="label" sortable>{{
                props.row.label
            }}</b-table-column>
            <b-table-column v-slot="props" label="Action" field="action" sortable>{{
                props.row.action
            }}</b-table-column>
            <b-table-column v-slot="props" label="Scope" field="scope" sortable>{{ props.row.scope }}</b-table-column>
            <b-table-column v-slot="props">
                <update-delete-dropdown @update="onUpdate(props.row)" @delete="onDelete(props.row)" size="is-small" />
            </b-table-column>

            <template slot="empty">
                <div class="is-flex is-justify-content-center">There's nothing here!</div>
            </template>
        </b-table>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Permission } from '@/api/users';
import { confirmDelete, toast, displayError, displayLoading } from '@/core';
import securityStore from '../../store/security-store';

@Component({
    name: 'permissions'
})
export default class Permissions extends Vue {
    get permissions(): Permission[] {
        return securityStore.permissions;
    }

    @displayLoading
    async created() {
        await securityStore.init();
    }

    async onUpdate(p: Permission) {
        this.$router.push({ name: 'updatePermission', params: { id: p.id } });
    }

    @displayLoading
    async onDelete(p: Permission) {
        const del = await confirmDelete('permission', p.label);

        if (del) {
            try {
                await securityStore.deletePermission(p);
                toast(`Deleted permission ${p.label}`);
            } catch (err) {
                displayError(err);
            }
        }
    }
}
</script>
