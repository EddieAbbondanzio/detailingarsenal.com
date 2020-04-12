<template>
    <page :loading="loading">
        <template v-slot:header>
            <page-header
                title="Create new service"
                description="A new service that can be used to create appointments"
                :backButtonTo="{name: 'settings'}"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb name="Services" :to="{name: 'services'}" />
                        <breadcrumb name="Create" :to="{name: 'createService'}" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" :loading="loading">
            <input-text-field
                ref="nameField"
                rules="required|max:32"
                label="Name"
                :required="true"
                v-model="name"
                placeholder="Wash n Wax"
            />

            <input-text-field
                ref="descriptionField"
                label="Description"
                rules="max:256"
                v-model="description"
                type="textarea"
                placeholder="Exterior is hand washed and dried. Then waxed for 1 month of protection."
            />

            <b-field label="Pricing Method">
                <!-- Not a typo. Label will be inline if we dont. -->
                <b-field>
                    <b-radio-button
                        v-model="pricingMethod"
                        native-value="fixed"
                        @input="onPricingMethodInput"
                    >Fixed</b-radio-button>
                    <b-radio-button
                        v-model="pricingMethod"
                        native-value="by-vehicle-category"
                        @input="onPricingMethodInput"
                    >By vehicle category</b-radio-button>
                </b-field>
            </b-field>

            <div v-if="pricingMethod == 'fixed'">
                <b-field grouped>
                    <input-text-field
                        v-model.number="configs[0].price"
                        rules="min_value:0|decimal:2"
                        label="Price"
                        style="width: 50%;"
                    />
                    <input-text-field
                        v-model.number="configs[0].duration"
                        label="Duration"
                        style="width: 50%;"
                        rules="multipleOf:15"
                    />
                </b-field>
            </div>
            <div v-else>
                <div class="is-flex is-flex-row is-align-items-center">
                    <div>
                        <div class="is-flex is-flex-row is-align-items-center">
                            <div class="has-margin-x-1" style="width: 30%;">
                                <span class="has-text-weight-bold is-hidden-tablet">Vehicle cat.</span>
                                <span class="has-text-weight-bold is-hidden-mobile">Vehicle category</span>
                            </div>
                            <div class="has-margin-x-1" style="width: 30%;">
                                <span class="has-text-weight-bold">Price</span>
                            </div>
                            <div class="has-margin-x-1" style="width: 10%;">
                                <span class="has-text-weight-bold">Duration</span>
                            </div>
                        </div>

                        <div
                            class="is-flex is-flex-row is-align-items-center has-margin-y-1"
                            v-for="(val, i) in configs"
                            :key="i"
                        >
                            <input-select
                                class="has-margin-x-1"
                                v-model="val.vehicleCategory"
                                style="width: 30%; margin-bottom: 0px;"
                            >
                                <option
                                    v-for="vc in vehicleCategories"
                                    :key="vc.id"
                                    :value="vc"
                                >{{ vc.name }}</option>
                            </input-select>
                            <input-text-field
                                class="has-margin-x-1"
                                type="number"
                                v-model.number="val.price"
                                style="width: 30%; margin-bottom: 0px;"
                                label="Price"
                                :hideLabel="true"
                                rules="min_value:0|decimal:2"
                            />
                            <input-text-field
                                class="has-margin-x-1"
                                type="number"
                                v-model.number="val.duration"
                                style="width: 30%; margin-bottom: 0px;"
                                label="Duration"
                                :hideLabel="true"
                                rules="multipleOf:15"
                            />
                            <b-button
                                class="has-margin-x-1"
                                icon-left="delete"
                                type="is-danger"
                                style="width: 10%; height: 100%;"
                                @click="onDelete(val)"
                            />
                        </div>

                        <div>
                            <b-button
                                type="is-text"
                                @click="onAddAnother"
                                :disabled="!canAddAnother()"
                            >Add another</b-button>
                        </div>
                    </div>
                </div>
            </div>
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ValidationProvider } from 'vee-validate';
import { ValidationObserver } from 'vee-validate';
import { getModule } from 'vuex-module-decorators';
import SettingsStore from '../../../store/settings/settings-store';
import { SpecificationError, toast } from '@/core';
import { displayError } from '@/modules/app/utils/display-error/display-error';
import { VehicleCategory } from '../../../api';

@Component({
    name: 'create-service',
    components: {
        ValidationProvider,
        ValidationObserver
    }
})
export default class CreateService extends Vue {
    loading: boolean = false;
    name: string = '';
    description: string = '';
    configs: { vehicleCategory: VehicleCategory | null; price: number | null; duration: number | null }[] = [
        { vehicleCategory: null, price: null, duration: null }
    ];
    pricingMethod: string = 'fixed';

    get vehicleCategories() {
        const settingsStore = getModule(SettingsStore, this.$store);
        return settingsStore.vehicleCategories;
    }

    async created() {
        const settingsStore = getModule(SettingsStore, this.$store);
        await settingsStore.init();
    }

    onAddAnother() {
        this.addEmptyRow();
    }

    onDelete(val: { vehicleCategory: VehicleCategory | null; price: number | null; duration: number | null }) {
        // Don't allow deleting of the only row. Just clear it instead.
        if (this.configs.length == 1) {
            this.configs[0].vehicleCategory = null;
            this.configs[0].price = null;
            this.configs[0].duration = null;

            return;
        }

        this.configs = this.configs.filter(v => v.vehicleCategory != val.vehicleCategory);
    }

    onPricingMethodInput() {
        if (this.pricingMethod == 'fixed') {
            this.configs.splice(1);
        }
    }

    canAddAnother() {
        return this.configs.length < this.vehicleCategories.length;
    }

    addEmptyRow() {
        this.configs.push({ vehicleCategory: null, price: null, duration: null });
    }

    public async onSubmit() {
        try {
            this.loading = true;
            const settingsStore = getModule(SettingsStore, this.$store);
            await settingsStore.createService({
                name: this.name,
                description: this.description,
                configurations: this.configs.map(c => ({
                    vehicleCategoryId: c.vehicleCategory != null ? c.vehicleCategory.id : null,
                    price: c.price || 0,
                    duration: c.duration || 0
                }))
            });

            toast(`Created new service ${this.name}`);
            this.$router.push({ name: 'services' });
        } catch (err) {
            displayError(err);
        } finally {
            this.loading = false;
        }
    }
}
</script>
