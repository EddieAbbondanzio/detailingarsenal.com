<template>
    <b-field :label="label" grouped :class="required ? 'is-required' : ''">
        <input-text-field
            style="width: 120px"
            v-model.number="value.amount"
            @input="onInput()"
            :rules="valRules"
            label="Amount"
            :hideLabel="true"
        />
        <input-select v-model="value.unit" @input="onInput()" label="Unit" :hideLabel="true">
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

    get valRules() {
        if (this.rules != null) {
            return `${this.rules}|min_value:0`;
        } else {
            return '|min_value:0';
        }
    }

    @Prop()
    rules!: string | null;

    @Prop()
    label!: string | null;

    @Prop()
    required!: boolean;

    @Prop({ default: () => ({ amount: null, unit: 'in' }) })
    value!: Measurement;

    onInput() {
        this.$emit('input', this.value);
    }
}
</script>