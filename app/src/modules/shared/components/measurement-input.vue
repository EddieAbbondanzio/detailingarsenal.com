<template>
    <b-field :label="label" grouped :class="required ? 'is-required' : ''">
        <input-text-field
            style="width: 120px"
            v-model.number="safeValue.amount"
            @input="onInput()"
            :rules="valRules"
            label="Amount"
            :hideLabel="true"
        />
        <input-select v-model="safeValue.unit" @input="onInput()" label="Unit" :hideLabel="true" class="unit-select">
            <option v-for="unit in units" :key="unit" :value="unit">{{ unit }}</option>
        </input-select>
    </b-field>
</template>

<style lang="sass">
.unit-select
    width: 70px!important
</style>

<script lang="ts">
import { Measurement, MeasurementUnit } from '@/api/shared';
import Vue from 'vue';
import Component from 'vue-class-component';
import { Prop, Watch } from 'vue-property-decorator';

@Component({ name: 'measurement-input' })
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

    get safeValue() {
        if (this.value == null) {
            this.$emit('input', { amount: null, unit: MeasurementUnit.Inches });
            return { amount: null, unit: MeasurementUnit.Inches }; //hack
        }

        return this.value;
    }

    @Prop()
    rules!: string | null;

    @Prop()
    label!: string | null;

    @Prop()
    required!: boolean;

    @Prop()
    value!: Measurement | null;

    onInput() {
        this.$emit('input', this.value);
    }
}
</script>
