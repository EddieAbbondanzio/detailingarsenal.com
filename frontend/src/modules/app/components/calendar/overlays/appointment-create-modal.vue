<template>
    <b-modal :active.sync="isActive" :has-modal-card="true">
        <div class="modal-card">
            <div class="modal-card-head">
                <p class="is-size-3">Create appointment</p>
            </div>
            <div class="modal-card-body is-flex is-flex-column is-align-items-center">
                <input-form>
                    <input-select label="Service">
                        <option v-for="service in services" :key="service.id">{{ service.name }}</option>
                    </input-select>
                </input-form>
            </div>

            <div class="modal-card-foot has-background-white has-border-top-0 is-radiusless">
                <b-button type="is-primary">Create</b-button>
                <b-button type="is-light" @click="hide">Cancel</b-button>
            </div>
        </div>
    </b-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import store from '../../../../../core/store';
import calendarStore from '../../../store/calendar/calendar-store';
import settingsStore from '../../../store/settings/settings-store';

@Component({
    name: 'appointment-create-modal'
})
export default class AppointmentCreateModal extends Vue {
    get services() {
        return settingsStore.services;
    }

    isActive: boolean = false;

    unsub: (() => void) | null = null;

    created() {
        settingsStore.init();

        this.unsub = store.subscribe((m, s) => {
            if (m.type == 'calendar/SET_CREATE_STEP' && m.payload == 'details') {
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

    hide() {
        this.isActive = false;
        calendarStore.CLEAR_CREATE_STEP();
        calendarStore.CLEAR_BLOCKS();
    }
}
</script>