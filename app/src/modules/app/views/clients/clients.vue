<template>
    <page :loading="loading">
        <template v-slot:header>
            <page-header
                title="Clients"
                :description="`${count} ${count == 1 ? 'client' : 'clients'}`"
                icon="contacts"
                :backButton="false"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Clients" :to="{name: 'clients'}" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <create-button :to="{name: 'createClient' }" text="Create client" />
                </template>
            </page-header>
        </template>

        <b-input
            icon="magnify"
            placeholder="Type to search"
            @input="onSearchChange"
            class="has-margin-bottom-3"
        />
        <list size="is-compact">
            <list-item
                class="has-border-bottom-1-light"
                v-for="client in clients"
                :key="client.id"
                :title="client.name"
                :to="{name: 'client', params: {id: client.id}}"
            >
                <div
                    class="is-hidden-mobile is-flex is-flex-row has-margin-y-1"
                    style="min-height: 24px;"
                >
                    <phone :value="client.phone" v-if="client.phone != null" :disabled="true" />
                    <email :value="client.email" v-if="client.email != null" :disabled="true" />
                </div>
            </list-item>
        </list>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Client } from '../../api';
import clientsStore from '../../store/clients/clients-store';

@Component({
    name: 'clients'
})
export default class Clients extends Vue {
    public clients: Client[] = [];
    public loading: boolean = true;

    get count() {
        return clientsStore.clients.length;
    }

    public async created() {
        await clientsStore.init();
        this.loading = false;
        this.clients = clientsStore.clients;
    }

    public onSearchChange(input: string) {
        this.clients = clientsStore.clients.filter(c => c.name.toLowerCase().includes(input.toLowerCase()));
    }
}
</script>