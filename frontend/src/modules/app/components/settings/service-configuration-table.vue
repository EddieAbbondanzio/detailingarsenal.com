<template>
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
            v-for="(val, i) in value"
            :key="i"
        >
            <input-select
                class="has-margin-x-1"
                v-model="val.vehicleCategory"
                style="width: 30%; margin-bottom: 0px;"
            >
                <option v-for="vc in vehicleCategories" :key="vc.id" :value="vc">{{ vc.name }}</option>
            </input-select>
            <input-text-field
                class="has-margin-x-1"
                type="number"
                v-model="val.price"
                style="width: 30%; margin-bottom: 0px;"
            />
            <input-text-field
                class="has-margin-x-1"
                type="number"
                v-model="val.duration"
                style="width: 30%; margin-bottom: 0px;"
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
            <b-button type="is-text" @click="onAddAnother" :disabled="!canAddAnother()">Add another</b-button>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { VehicleCategory } from '../../api';
import { getModule } from 'vuex-module-decorators';
import SettingsStore from '../../store/settings/settings-store';

@Component({
    name: 'service-configuration-table'
})
export default class ServiceConfigurationTable extends Vue {
    @Prop()
    value: ServiceConfigurationTableRow[] = [];

    get vehicleCategories() {
        const settingsStore = getModule(SettingsStore, this.$store);
        return settingsStore.vehicleCategories;
    }

    async created() {
        const settingsStore = getModule(SettingsStore, this.$store);
        await settingsStore.init();
        this.addEmptyRow();
    }

    onAddAnother() {
        this.addEmptyRow();
    }

    onDelete(val: ServiceConfigurationTableRow) {
        // Don't allow deleting of the only row. Just clear it instead.
        if (this.value.length == 1) {
            // this.value[0].vehicleCategory = null;
            // this.value[0].price = null;
            // this.value[0].duration = null;

            return;
        }

        this.$emit(
            'input',
            this.value.filter(v => v.vehicleCategory == val.vehicleCategory)
        );
    }

    canAddAnother() {
        return this.value.length < this.vehicleCategories.length;
    }

    addEmptyRow() {
        this.$emit('input', { vehicleCategory: null, price: null, duration: null });
    }
}

export type ServiceConfigurationTableRow = {
    vehicleCategory: VehicleCategory | null;
    price: number | null;
    duration: number | null;
};
</script>