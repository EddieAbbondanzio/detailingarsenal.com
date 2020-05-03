<template>
    <b-modal :active.sync="isActive" :has-modal-card="true" @close="hide(true)">
        <div class="card" v-if="value != null">
            <div class="modal-card-body">
                <div class="is-size-5">{{ service.name }} - {{ value.price | currency }}</div>

                <div>
                    <div
                        v-for="block in value.blocks"
                        :key="block.id"
                    >{{ block.start | date }} {{ block.start | twelveHourFormat }} - {{ block.end | twelveHourFormat }}</div>
                </div>
            </div>
        </div>
    </b-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import calendarStore from '../../../store/calendar/calendar-store';
import store from '../../../../../core/store';
import { AppointmentBlock } from '../../../api/calendar/entities/appointment-block';
import settingsStore from '../../../store/settings/settings-store';

@Component({
    name: 'appointment-details-modal'
})
export default class AppointmentDetailsModal extends Vue {
    get service() {
        return settingsStore.services.find(s => s.id == this.value!.serviceId);
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
}
</script>