<template>
    <page>
        <template v-slot:header>
            <page-header title="Create role" :description="`Create new role`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Users panel" :to="{ name: 'usersPanel' }" />
                        <breadcrumb name="Roles" :to="{ name: 'roles' }" />
                        <breadcrumb
                            v-if="mode == 'update'"
                            :name="name"
                            :to="{ name: 'role', params: $route.params }"
                        />
                        <breadcrumb :name="verb" :to="$route" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" :submitText="verb">
            <input-text-field label="Name" rules="required|max:32" :required="true" v-model="name" placeholder="user" />

            <b-field label="Permissions">
                <b-table :data="permissions" checkable :checked-rows.sync="enabledPermissions">
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
            </b-field>
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Permission, SpecificationError } from '@/api';
import { displayError, toast, displayLoading } from '@/core';
import securityStore from '../../store/security-store';
import InputViewMixin from '@/core/mixins/input-view-mixin';

@Component
export default class CreateRole extends InputViewMixin {
    get permissions() {
        return securityStore.permissions;
    }

    name = '';
    enabledPermissions: Permission[] = [];

    @displayLoading
    async created() {
        await securityStore.init();

        if (this.mode == 'update') {
            const role = securityStore.roles.find((r) => r.id == this.$route.params.id);

            if (role == null) {
                this.$router.go(-1);
                return;
            }

            this.name = role.name;
            this.enabledPermissions = role.permissions;
        }
    }

    @displayLoading
    async onSubmit() {
        try {
            if (this.mode == 'create') {
                const create = {
                    name: this.name,
                    permissionIds: this.enabledPermissions.map((p) => p.id),
                };
                const role = await securityStore.createRole(create);
            } else {
                const update = {
                    id: this.$route.params.id,
                    name: this.name,
                    permissionIds: [...new Set(this.enabledPermissions.map((p) => p.id))],
                };

                const role = await securityStore.updateRole(update);
            }

            toast(`${this.verb} role ${this.name}`);
            this.$router.push({ name: 'roles' });
        } catch (err) {
            throw err;
        }
    }
}
</script>