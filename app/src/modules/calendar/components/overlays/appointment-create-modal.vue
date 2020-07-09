<template>
    <b-modal
        :active.sync="isActive"
        :has-modal-card="true"
        :can-cancel="canCancel"
        @close="hide(true)"
    >
        <div class="card">
            <div class="modal-card-head">
                <p class="is-size-3">Create appointment</p>
            </div>
            <div
                class="modal-card-body has-padding-all-0 is-flex is-flex-row is-align-items-center"
            >
                <input-form
                    class="is-flex-grow-1"
                    :preventCancelDefault="true"
                    @submit="submit()"
                    @cancel="hide()"
                >
                    <div>
                        <p class="is-size-5">Details</p>
                        <hr class="has-margin-top-0 has-padding-y-0 has-margin-bottom-3" />
                    </div>

                    <input-group multiline>
                        <input-select
                            label="Service"
                            :required="true"
                            rules="required"
                            v-model="service"
                            @input="() => { price = null; populateServiceDetails() }"
                        >
                            <option
                                v-for="service in services"
                                :key="service.id"
                                :value="service"
                            >{{ service.name }}</option>
                        </input-select>

                        <input-select
                            label="Vehicle Category"
                            :required="true"
                            v-model="vehicleCategory"
                            v-if="service != null && isVariablePrice()"
                            @input="populateServiceDetails()"
                        >
                            <option
                                v-for="vc in vehicleCategories"
                                :key="vc.id"
                                :value="vc"
                            >{{ vc.name }}</option>
                        </input-select>
                    </input-group>

                    <input-group multiline>
                        <input-text-field
                            label="Price"
                            :required="true"
                            rules="required|decimal:2"
                            v-model.number="price"
                        />

                        <input-text-field
                            label="Estimated Time"
                            v-model="estimatedTime"
                            :disabled="true"
                            title="Estimated time from service configuration"
                        />
                    </input-group>

                    <!-- Times -->
                    <div>
                        <p class="is-size-5">Time(s)</p>
                        <hr class="has-margin-top-0 has-padding-y-0 has-margin-bottom-3" />
                    </div>

                    <div class="has-margin-bottom-3">
                        <div v-if="blocks.length > 1"></div>

                        <div
                            class="is-flex is-flex-row is-align-items-center has-margin-bottom-1"
                            v-for="(block, i) in blocks"
                            :key="i"
                        >
                            <input-datepicker
                                class="has-margin-x-1 has-margin-y-0"
                                title="Start date"
                                :value="block.start"
                                @input="inp => updateBlockStart(block, inp)"
                            />

                            <input-timepicker
                                class="has-margin-x-1 has-margin-y-0"
                                title="Start time"
                                :value="block.start"
                                @input="inp => updateBlockStart(block, inp)"
                            />

                            <span class="has-margin-x-1">to</span>

                            <input-timepicker
                                class="has-margin-x-1 has-margin-y-0"
                                title="End time"
                                :value="block.end"
                                @input="inp => updateBlockEnd(block, inp)"
                            />

                            <a
                                class="delete has-margin-x-1"
                                @click="deleteBlock(block)"
                                v-if="blocks.length > 1"
                            ></a>
                        </div>

                        <b-button
                            class="has-margin-top-3"
                            type="is-primary"
                            @click="onCalendarClick"
                        >Select on calendar</b-button>
                    </div>

                    <div>
                        <p class="is-size-5">Client</p>
                        <hr class="has-margin-top-0 has-padding-y-0 has-margin-bottom-3" />
                    </div>

                    <b-field label="Name" class="is-required">
                        <validation-provider
                            name="Name"
                            rules="required"
                            v-slot="{ classes, errors }"
                        >
                            <b-autocomplete
                                icon="account"
                                :class="classes"
                                v-model="client.name"
                                :data="filteredClients"
                                :open-on-focus="true"
                                @select="onClientSelected"
                                @blur="onClientBlur"
                                field="name"
                            >
                                <template v-slot="{ option }">
                                    <div style="height: 42px">
                                        <p>{{ option.name }}</p>
                                        <span v-if="option.phone != null">
                                            <b-icon icon="phone" size="is-small" />
                                            {{ option.phone }}
                                        </span>
                                        <span v-if="option.email != null">
                                            <b-icon icon="email" size="is-small" />
                                            {{ option.phone }}
                                        </span>
                                    </div>
                                </template>
                            </b-autocomplete>
                            <input-error-message :text="errors[0]" />
                        </validation-provider>
                    </b-field>

                    <div
                        v-if="isNewClient"
                        class="is-flex is-flex-row is-align-items-center has-text-success"
                    >
                        <b-icon icon="account-plus" class="has-margin-right-1" />New client will be created
                    </div>

                    <input-group multiline>
                        <input-text-field iconLeft="phone" label="Phone" v-model="client.phone" />
                        <input-text-field iconLeft="email" label="Email" v-model="client.email" />
                    </input-group>

                    <hr class="has-margin-top-0 has-padding-y-0 has-margin-bottom-3" />

                    <input-text-field
                        label="Notes"
                        v-model="notes"
                        rules="max:1024"
                        :maxLength="1024"
                        type="textarea"
                    />
                </input-form>
            </div>
        </div>
    </b-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { duration, displayError } from '@/core';
import settingsStore from '@/modules/settings/store/settings-store';
import clientsStore from '@/modules/clients/store/clients-store';
import calendarStore from '../../store/calendar-store';
import store from '@/core/store';
import {
    VehicleCategory,
    Service,
    ServiceConfiguration,
    Client,
    AppointmentBlock,
    AppointmentBlockCreate
} from '@/api';

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

    get canCancel() {
        if (this.service != null || this.price != null) {
            return ['x'];
        } else {
            return ['x', 'escape', 'outside'];
        }
    }

    get blocks() {
        return calendarStore.pendingBlocks;
    }

    get filteredClients() {
        if (this.client.name == null) {
            return clientsStore.clients;
        } else {
            return clientsStore.search(this.client.name);
        }
    }

    get isNewClient() {
        return this.client.name != null && this.client.id == null;
    }

    isActive: boolean = false;
    unsub: (() => void) | null = null;

    service: Service | null = null;
    vehicleCategory: VehicleCategory | null = null;
    price: number | null = null;
    estimatedTime: string | null = null;
    notes: string | null = null;

    client: {
        id: string | null;
        name: string | null;
        phone: string | null;
        email: string | null;
    } = { id: null, name: null, phone: null, email: null };

    created() {
        settingsStore.init();
        clientsStore.init();

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

    hide(clear: boolean = true) {
        this.isActive = false;

        if (clear) {
            this.service = null;
            this.price = null;
            this.estimatedTime = null;

            calendarStore.cancelPendingChanges();
        }
    }

    populateServiceDetails() {
        if (this.service == null) {
            return null;
        }

        let config: ServiceConfiguration | null = null;

        if (this.service.pricingMethod == 'Fixed') {
            config = this.service.configurations[0];
        } else if (this.vehicleCategory != null) {
            config = this.service.getConfigurationForVehicleCategory(this.vehicleCategory)!;
        }

        if (config != null) {
            this.price = config.price;
            this.estimatedTime = duration(config.duration);
        } else {
            this.price == null;
            this.estimatedTime = null;
        }
    }

    isVariablePrice() {
        return this.service!.pricingMethod != 'Fixed';
    }

    async submit() {
        try {
            let clientId = this.client.id!;

            if (this.isNewClient) {
                var c = await clientsStore.createClient({
                    name: this.client.name!,
                    email: this.client.email!,
                    phone: this.client.phone!
                });

                clientId = c.id;
            }

            let a = await this.createAppointment(this.service!.id, this.price!, clientId, this.blocks, this.notes!);
            this.hide();
        } catch (err) {
            displayError(err);
        }
    }

    onCalendarClick() {
        calendarStore.SET_CREATE_STEP('selections');
        this.hide(false);
    }

    onClientSelected(opt: Client | null) {
        if (opt == null) {
            this.client.id = null;
            this.client.name = null;
            this.client.phone = null;
            this.client.email = null;
        } else {
            this.client.id = opt.id;
            this.client.phone = opt.phone;
            this.client.email = opt.email;
        }
    }

    onClientBlur() {
        if (this.client.id == null) {
            this.client.phone = null;
            this.client.email = null;
        }
    }

    updateBlockStart(block: AppointmentBlock, date: Date) {
        calendarStore.UPDATE_BLOCK_START({ block, start: date });
    }

    updateBlockEnd(block: AppointmentBlock, date: Date) {
        calendarStore.UPDATE_BLOCK_END({ block, end: date });
    }

    deleteBlock(block: AppointmentBlock) {
        calendarStore.DELETE_BLOCK(block);
    }

    async createAppointment(
        serviceId: string,
        price: number,
        clientId: string,
        blocks: AppointmentBlockCreate[],
        notes: string
    ) {
        return calendarStore.createAppointment({
            serviceId,
            price,
            clientId,
            blocks,
            notes
        });
    }
}
</script>