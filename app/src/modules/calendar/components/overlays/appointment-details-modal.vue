<template>
    <b-modal :active.sync="isActive" :has-modal-card="true" @close="hide(true)">
        <div class="card" v-if="value != null">
            <div class="modal-card-body">
                <div
                    class="is-flex is-flex-row is-justify-content-space-between has-margin-bottom-3"
                >
                    <div class="is-size-5">{{ service.name }}</div>

                    <div>
                        <b-button type="is-text" icon-left="pencil"></b-button>
                        <b-button
                            type="is-text"
                            class="has-text-danger"
                            icon-left="delete"
                            @click="onDelete"
                        ></b-button>
                    </div>
                </div>

                <div class="is-flex is-flex-column">
                    <div class="is-flex is-flex-row has-margin-y-1">
                        <b-icon icon="calendar" class="has-margin-right-1" />
                        <div v-for="block in value.blocks" :key="block.id">
                            {{ block.start | date }} {{ block.start | twelveHourFormat }} -
                            {{ block.end | twelveHourFormat }}
                        </div>
                    </div>
                    <router-link
                        class="is-flex is-flex-row has-margin-y-1 has-text-dark"
                        :to="{ name: 'client', params: { id: client.id } }"
                    >
                        <b-icon icon="account" class="has-margin-right-1" />
                        {{ client.name }}
                    </router-link>

                    <div class="is-flex is-flex-row has-margin-y-1">
                        <b-icon icon="currency-usd" class="has-margin-right-1" />
                        {{ value.price | currency }}
                    </div>

                    <div class="is-flex is-flex-row has-margin-y-1">
                        <b-icon icon="timelapse" class="has-margin-right-1" />
                        {{ value.duration | duration }}
                    </div>
                </div>
            </div>
        </div>
    </b-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import calendarStore from '../../store/calendar-store';
import { confirmDelete, displayError } from '@/core';
import store from '@/core/store';
import clientsStore from '@/modules/clients/store/clients-store';
import settingsStore from '@/modules/settings/store/settings-store';

@Component({
    name: 'appointment-details-modal'
})
export default class AppointmentDetailsModal extends Vue {
    get service() {
        return settingsStore.services.find(s => s.id == this.value!.serviceId);
    }

    get client() {
        return clientsStore.clients.find(c => c.id == this.value!.clientId);
    }

    get value() {
        return calendarStore.active;
    }

    isActive: boolean = false;
    unsub: (() => void) | null = null;

    created() {
        this.unsub = store.subscribe((m, s) => {
            if (m.type == 'calendar/SET_ACTIVE_APPOINTMENT') {
                this.show();
            }
        });
    }

    beforeDestroy() {
        if (this.unsub != null) {
            this.unsub();
        }
    }

    show() {
        this.isActive = true;
    }

    hide(clear: boolean = true) {
        this.isActive = false;
        calendarStore.CLEAR_ACTIVE_APPOINTMENT();
    }

    async onDelete() {
        const userWantsToDelete = await confirmDelete('appointment', this.service!.name);

        if (userWantsToDelete) {
            try {
                await calendarStore.deleteAppointment(this.value!);
            } catch (err) {
                displayError(err);
            }
        }
    }
}
</script>
