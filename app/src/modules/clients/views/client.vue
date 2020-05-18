<template>
    <page :loading="loading">
        <template v-slot:header v-if="client != null">
            <page-header>
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Clients" :to="{name: 'clients'}" />
                        <breadcrumb
                            :name="client.name"
                            :to="{name: 'client', params: $route.params}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <edit-delete-dropdown @edit="onEdit" @delete="onDelete" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless is-flex is-flex-column is-flex-grow-1">
            <div class="is-flex is-flex-column is-align-items-center has-margin-y-3">
                <!-- User Icon -->
                <div
                    class="box is-shadowless has-background-light has-padding-all-3 is-inline-block"
                >
                    <b-icon icon="account" type="is-dark" size="is-large" />
                </div>

                <p class="is-size-4 has-text-weight-bold">{{ client.name }}</p>
            </div>

            <!-- Call / Text / Email quick actions -->
            <div
                class="is-hidden-tablet has-margin-y-3 is-flex is-flex-row is-justify-content-space-around is-hidden-tablet"
            >
                <b-button
                    type="is-success"
                    icon-left="phone"
                    size="is-large"
                    tag="a"
                    :href="`tel:${client.phone}`"
                    :disabled="client.phone == null"
                ></b-button>
                <b-button
                    type="is-primary"
                    icon-left="message"
                    size="is-large"
                    tag="a"
                    :href="`sms:${client.phone}`"
                    :disabled="client.phone == null"
                ></b-button>
                <b-button
                    class="has-background-purple has-text-white"
                    icon-left="email"
                    size="is-large"
                    tag="a"
                    :href="`mailto:${client.email}`"
                    :disabled="client.email == null"
                ></b-button>
            </div>

            <div class="is-flex is-flex-column is-align-items-center has-margin-top-3">
                <phone class="has-margin-y-2" :value="client.phone" v-if="client.phone != null" />
                <email class="has-margin-y-2" :value="client.email" v-if="client.email != null" />
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import ClientWidget from '@/modules/clients/components/client-widget.vue';
import clientsStore from '../store/clients-store';
import { confirmDelete, displayError, toast } from '../../../core';

@Component({
    name: 'client',
    components: {
        ClientWidget
    }
})
export default class Client extends Vue {
    loading = false;

    get client() {
        return clientsStore.clients.find(c => c.id == this.$route.params.id)!;
    }

    async created() {
        this.loading = true;
        await clientsStore.init();
        this.loading = false;
    }

    onEdit() {
        this.$router.push({ name: 'editClient', params: { id: this.client.id } });
    }

    async onDelete() {
        const deleteConfirmed = await confirmDelete('client', this.client.name);

        if (deleteConfirmed) {
            try {
                this.loading = true;
                await clientsStore.deleteClient(this.client);

                toast(`Deleted client ${this.client.name}`);
                this.$router.push({ name: 'clients' });
            } catch (err) {
                displayError(err);
            } finally {
                this.loading = false;
            }
        }
    }
}
</script>