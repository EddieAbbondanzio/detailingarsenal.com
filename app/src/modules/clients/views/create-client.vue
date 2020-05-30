<template>
    <page>
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

        <input-form @submit="onSubmit" submitText="Create">
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
import { toast, displayError } from '@/core';
import clientsStore from '../store/clients-store';
import { displayLoading } from '../../../core/utils/display-loading';

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

    @displayLoading
    async onSubmit() {
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
        }
    }
}
</script>