<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Permissions"
                description="Access control"
                icon="lock"
                :backButtonTo="{name: 'adminPanel'}"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Permissions" :to="{name: 'permissions'}" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{name: 'createPermission' }" text="Create permission" />
                </template>
            </page-header>
        </template>

        <b-table :data="permissions">
            <template slot-scope="props">
                <b-table-column label="Permission" field="permission" sortable>{{ props.row.label }}</b-table-column>
                <b-table-column label="Action" field="action" sortable>{{ props.row.action }}</b-table-column>
                <b-table-column label="Scope" field="scope" sortable>{{ props.row.scope }}</b-table-column>
                <b-table-column>
                    <edit-delete-dropdown
                        @edit="onEdit(props.row)"
                        @delete="onDelete(props.row)"
                        size="is-small"
                    />
                </b-table-column>
            </template>

            <template slot="empty">
                <div class="is-flex is-justify-content-center">There's nothing here!</div>
            </template>
        </b-table>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Permission } from '@/modules/admin/api/entities/permission';
import { confirmDelete, toast, displayError } from '@/core';
import adminStore from '../../store/admin-store';
import { displayLoading } from '../../../../core/utils/display-loading';

@Component({
    name: 'permissions'
})
export default class Permissions extends Vue {
    get permissions(): Permission[] {
        return adminStore.permissions;
    }

    @displayLoading
    async created() {
        await adminStore.init();
    }

    async onEdit(p: Permission) {
        this.$router.push({ name: 'editPermission', params: { id: p.id } });
    }

    @displayLoading
    async onDelete(p: Permission) {
        const del = await confirmDelete('permission', p.label);

        if (del) {
            try {
                await adminStore.deletePermission(p);
                toast(`Deleted permission ${p.toString()}`);
            } catch (err) {
                displayError(err);
            }
        }
    }
}
</script>