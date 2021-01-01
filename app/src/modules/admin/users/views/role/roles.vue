<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Roles"
                description="Collection of permissions"
                icon="lock"
                :backButtonTo="{ name: 'adminPanel' }"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'usersPanel' }" />
                        <breadcrumb name="Roles" :to="{ name: 'roles' }" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{ name: 'createRole' }" text="Create role" />
                </template>
            </page-header>
        </template>

        <list>
            <list-item v-for="r in roles" :key="r.id" :title="r.name" :to="{ name: 'role', params: { id: r.id } }">
                <template v-slot:actions>
                    <update-delete-dropdown @update="onUpdate(r)" @delete="onDelete(r)" />
                </template>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { displayLoading } from '@/core/utils/display-loading';
import { confirmDelete, toast, displayError } from '@/core';
import { Role } from '@/api';
import securityStore from '../../store/security-store';

@Component({
    name: 'roles',
})
export default class Roles extends Vue {
    get roles() {
        return securityStore.roles;
    }

    @displayLoading
    async created() {
        await securityStore.init();
    }

    async onUpdate(r: Role) {
        this.$router.push({ name: 'updateRole', params: { id: r.id } });
    }

    @displayLoading
    async onDelete(r: Role) {
        const del = await confirmDelete('role', r.name);

        if (del) {
            try {
                await securityStore.deleteRole(r);
                toast(`Deleted role ${r.name}`);
            } catch (err) {
                displayError(err);
            }
        }
    }
}
</script>