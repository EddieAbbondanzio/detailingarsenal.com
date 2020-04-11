<template>
    <page :loading="loading" v-if="client != null">
        <template v-slot:header>
            <page-header :title="`Edit ${name}`" description="Edit an existing client">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Clients" :to="{name: 'clients'}" />
                        <breadcrumb :name="name" :to="{name: 'client', params: {id: client.id }}" />
                        <breadcrumb
                            name="Edit"
                            :to="{name: 'editClient', params: { id: client.id }}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes" :loading="loading">
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
import { getModule } from 'vuex-module-decorators';
import ClientsStore from '../../store/clients/clients-store';
import { toast } from '../../../../core';
import { displayError } from '../../utils/display-error/display-error';

@Component({
    name: 'create-client',
    components: {
        ValidationProvider
    }
})
export default class CreateClient extends Vue {
    get client() {
        const clientStore = getModule(ClientsStore, this.$store);
        return clientStore.clients.find(c => c.id == this.$route.params.id)!;
    }

    name: string = '';
    phone: string = '';
    email: string = '';
    loading: boolean = false;

    async created() {
        const clientStore = getModule(ClientsStore, this.$store);
        await clientStore.init();

        const c = await clientStore.clients.find(c => c.id == this.$route.params.id);

        if (c == null) {
            throw new Error(`No client found for ${this.$route.params.id}`);
        }

        this.name = c.name;
        this.phone = c.phone || '';
        this.email = c.email || '';
    }

    async onSubmit() {
        this.loading = true;

        try {
            const clientStore = getModule(ClientsStore, this.$store);
            await clientStore.updateClient({
                id: this.$route.params.id,
                name: this.name,
                phone: this.phone,
                email: this.email
            });

            toast(`Edited client ${this.name}`);
            this.$router.push({ name: 'clients' });
        } catch (err) {
            displayError(err);
        } finally {
            this.loading = false;
        }
    }
}
</script>