<template>
    <b-field :label="label" grouped>
        <input-text-field
            style="width: 120px"
            v-model.number="value.amount"
            @input="onInput()"
            rules="required|min_value:0"
            label="Amount"
            :hideLabel="true"
        />
        <input-select v-model="value.unit" @input="onInput()" rules="required" label="Unit" :hideLabel="true">
            <option v-for="unit in units" :key="unit" :value="unit">{{ unit }}</option>
        </input-select>
    </b-field>
</template>

<script lang="ts">
import { Measurement, MeasurementUnit } from '@/api';
import Vue from 'vue';
import Component from 'vue-class-component';
import { Prop } from 'vue-property-decorator';

@Component
export default class MeasurementInput extends Vue {
    get units() {
        return [MeasurementUnit.Inches, MeasurementUnit.Millimeters];
    }

    @Prop()
    label!: string | null;

    @Prop({ default: () => ({ amount: null, unit: 'in' }) })
    value!: Measurement;

    onInput() {
        this.$emit('input', this.value);
    }
}
</script>