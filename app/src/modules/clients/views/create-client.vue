<template>
    <page :loading="loading">
        <template v-slot:header>
            <page-header title="Create new client" description="Create a new client for services">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Clients" :to="{name: 'clients'}" />
                        <breadcrumb name="Create" :to="{name: 'createClient'}" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Create" :loading="loading">
            <input-text-field
                label="Name"
                required="true"
                rules="required|max:64"
                v-model="name"
                placeholder="John Smith"
            />

            <input-text-field
                label="Phone"
                rules="max:32"
                placeholder="123-123-1234"
                v-model="phone"
            />

            <input-text-field
                label="Email"
                rules="email|max:320"
                v-model="email"
                placeholder="leroy.jenkins@mail.com"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ValidationProvider } from 'vee-validate';
import { toast } from '../../../../core';
import { displayError } from '../../utils/display-error/display-error';
import clientsStore from '../../store/clients/clients-store';

@Component({
    name: 'create-client',
    components: {
        ValidationProvider
    }
})
export default class CreateClient extends Vue {
    name: string = '';
    phone: string = '';
    email: string = '';
    loading: boolean = false;

    async onSubmit() {
        this.loading = true;

        try {
            await clientsStore.createClient({
                name: this.name,
                phone: this.phone,
                email: this.email
            });

            toast(`Created new client ${this.name}`);
            this.$router.push({ name: 'clients' });
        } catch (err) {
            displayError(err);
        } finally {
            this.loading = false;
        }
    }
}
</script>