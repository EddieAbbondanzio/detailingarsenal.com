<template>
    <b-modal :active.sync="isActive" :has-modal-card="true">
        <div class="modal-card">
            <div class="modal-card-head">
                <p class="is-size-3">Create appointment</p>
            </div>
            <div class="modal-card-body is-flex is-flex-row is-align-items-center">
                <input-form class="is-flex-grow-1">
                    <input-group>
                        <input-select label="Service" v-model="service">
                            <option
                                v-for="service in services"
                                :key="service.id"
                                :value="service"
                            >{{ service.name }}</option>
                        </input-select>

                        <input-select
                            label="Vehicle Category"
                            v-model="vehicleCategory"
                            v-if="service != null && isVariablePrice()"
                        >
                            <option
                                v-for="vc in vehicleCategories"
                                :key="vc.id"
                                :value="vc"
                            >{{ vc.name }}</option>
                        </input-select>
                    </input-group>

                    <div class="column is-4">
                        <input-text-field label="Price" v-model.number="price" />
                    </div>

                    <div class="column is-4">
                        <input-text-field
                            label="Estimated Time"
                            v-model="estimatedTime"
                            :disabled="true"
                        />
                    </div>

                    <div class="column is-8">
                        <input-text-field label="Name" />
                    </div>

                    <div class="column is-4">
                        <input-text-field label="Phone" />
                    </div>
                    <div class="column is-4">
                        <input-text-field label="Email" />
                    </div>
                </input-form>
            </div>
        </div>
    </b-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import store from '../../../../../core/store';
import calendarStore from '../../../store/calendar/calendar-store';
import settingsStore from '../../../store/settings/settings-store';
import { Service, VehicleCategory } from '../../../api';

@Component({
    name: 'appointment-create-modal'
})
export default class AppointmentCreateModal extends Vue {
    get services() {
        return settingsStore.services;
    }

    get vehicleCategories() {
        if (this.service == null) {
            return [];
        } else {
            return this.service.configurations.map(c =>
                settingsStore.vehicleCategories.find(vc => vc.id == c.vehicleCategoryId)
            );
        }
    }

    isActive: boolean = false;
    unsub: (() => void) | null = null;

    service: Service | null = null;
    vehicleCategory: VehicleCategory | null = null;
    price: number | null = null;
    estimatedTime: number | null = null;

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

    isVariablePrice() {
        return this.service!.pricingMethod != 'Fixed';
    }
}
</script>