<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Roles"
                description="Collection of permissions"
                icon="lock"
                :backButtonTo="{name: 'adminPanel'}"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb name="Roles" :to="{name: 'roles'}" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{name: 'createRole' }" text="Create role" />
                </template>
            </page-header>
        </template>

        <list>
            <list-item
                v-for="r in roles"
                :key="r.id"
                :title="r.name"
                :to="{name: 'role', params: { id: r.id}}"
            >
                <template v-slot:actions>
                    <edit-delete-dropdown @edit="onEdit(r)" @delete="onDelete(r)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import adminStore from '../../store/admin-store';
import { displayLoading } from '@/core/utils/display-loading';
import { Role } from '../../api/entities/role';
import { Permission } from '../../api/entities/permission';
import { confirmDelete, toast, displayError } from '../../../../core';

@Component({
    name: 'roles'
})
export default class Roles extends Vue {
    get roles() {
        return adminStore.roles;
    }

    @displayLoading
    async created() {
        await adminStore.init();
    }

    async onEdit(r: Role) {
        this.$router.push({ name: 'editRole', params: { id: r.id } });
    }

    @displayLoading
    async onDelete(r: Role) {
        const del = await confirmDelete('role', r.name);

        if (del) {
            try {
                await adminStore.deleteRole(r);
                toast(`Deleted role ${r.name}`);
            } catch (err) {
                displayError(err);
            }
        }
    }
}
</script>