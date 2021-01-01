<template>
    <page>
        <template v-slot:header>
            <page-header :title="role != null ? role.name : ``">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'usersPanel' }" />
                        <breadcrumb name="Roles" :to="{ name: 'roles' }" />
                        <breadcrumb
                            :name="role != null ? role.name : ''"
                            :to="{ name: 'role', params: $route.params }"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <update-button :to="{ name: 'editRole', params: { id: $route.params.id } }" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="role != null">
            <div class="has-margin-bottom-4">
                <h5 class="is-size-4 title has-margin-bottom-2">{{ role.name }}</h5>
            </div>

            <b-field label="Permissions">
                <b-table
                    :data="permissions"
                    checkable
                    :checked-rows.sync="enabledPermissions"
                    :custom-is-checked="
                        (a, b) => {
                            return a.id === b.id;
                        }
                    "
                    :is-row-checkable="(r) => false"
                >
                    <b-table-column v-slot="props" label="Permission" field="label" sortable>{{
                        props.row.label
                    }}</b-table-column>
                    <b-table-column v-slot="props" label="Action" field="action" sortable>{{
                        props.row.action
                    }}</b-table-column>
                    <b-table-column v-slot="props" label="Scope" field="scope" sortable>{{
                        props.row.scope
                    }}</b-table-column>

                    <template slot="empty">
                        <div class="is-flex is-justify-content-center">There's nothing here!</div>
                    </template>
                </b-table>
            </b-field>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import appStore from '@/core/store/app-store';
import { Permission, Role } from '@/api';
import securityStore from '../../store/security-store';
import { displayLoading } from '@/core';

@Component({
    name: 'role',
})
export default class RoleView extends Vue {
    get permissions() {
        return securityStore.permissions;
    }

    role: Role | null = null;
    enabledPermissions: Permission[] = [];

    @displayLoading
    async created() {
        await securityStore.init();

        this.role = securityStore.roles.find((r) => r.id == this.$route.params.id)!;

        if (this.role == null) {
            throw new Error('Role not found');
        }

        this.enabledPermissions = this.role.permissions;
    }
}
</script>