<template>
    <b-field :label="hideLabel ? null : label" :class="required ? 'is-required' : ''">
        <template v-slot:label>
            <slot name="label"></slot>
        </template>

        <validation-provider
            :vid="vid"
            :name="label"
            :rules="rules"
            v-slot="{ errors, classes }"
            ref="validator"
        >
            <b-slider
                :type="type"
                :class="classes"
                :value="value"
                :disabled="disabled"
                :min="min"
                :max="max"
                @input="onInput"
            >
                <b-slider-tick
                    v-for="tick in ticks"
                    :key="tick.value"
                    :value="tick.value"
                >{{tick.label}}</b-slider-tick>
            </b-slider>
            <input-error-message v-if="!hideErrors" :text="errors[0]" />
        </validation-provider>
    </b-field>
</template>

<style lang="sass" scoped>
.b-slider-tick
    text-align: center!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-slider'
})
export default class InputSlider extends Vue {
    @Prop({ default: null })
    id!: string | null;

    @Prop()
    label!: string;

    @Prop()
    required!: boolean;

    @Prop()
    rules!: string;

    @Prop()
    value!: number;

    @Prop({ default: 0 })
    min!: number;

    @Prop({ default: 100 })
    max!: number;

    @Prop({ default: false })
    disabled!: boolean;

    @Prop({ default: false })
    hideErrors!: boolean;

    @Prop({ default: false })
    hideLabel!: boolean;

    @Prop({ default: 'is-primary' })
    type!: string;

    @Prop()
    ticks!: InputSliderTick[];

    get vid() {
        if (this.id != null) {
            return this.id;
        } else {
            return this.label;
        }
    }

    onInput(val: any) {
        this.$emit('input', val);
    }
}

export type InputSliderTick = { value: any; label: string };
</script>