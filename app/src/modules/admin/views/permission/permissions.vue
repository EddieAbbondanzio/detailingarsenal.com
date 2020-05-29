<template>
    <page :loading="loading">
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

        <list>
            <list-item v-for="p in permissions" :key="p.id" :title="p.toString()">
                <template v-slot:actions>
                    <edit-delete-dropdown @edit="onEdit(p)" @delete="onDelete(p)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Permission } from '@/modules/admin/api/entities/permission';
import { confirmDelete, toast, displayError } from '@/core';
import adminStore from '../../store/admin-store';

@Component({
    name: 'permissions'
})
export default class Permissions extends Vue {
    loading: boolean = true;

    get permissions() {
        return adminStore.permissions;
    }

    async created() {
        this.loading = true;
        await adminStore.init();
        this.loading = false;
    }

    async onEdit(p: Permission) {
        this.$router.push({ name: 'editPermission', params: { id: p.id } });
    }

    async onDelete(p: Permission) {
        const del = await confirmDelete('permission', p.toString());

        if (del) {
            try {
                this.loading = true;
                await adminStore.deletePermission(p);
                toast(`Delete permission ${p.toString()}`);
            } catch (err) {
                displayError(err);
            } finally {
                this.loading = false;
            }
        }
    }
}
</script>