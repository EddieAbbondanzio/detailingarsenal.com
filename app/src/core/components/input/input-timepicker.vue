<template>
    <b-field :label="hideLabel ?  null : label" :class="required ? 'is-required' : ''">
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
            <b-timepicker
                icon="clock-outline"
                :class="classes"
                :value="value"
                :placeholder="placeholder"
                :disabled="disabled"
                editable
                hour-format="12"
                :increment-minutes="15"
                @input="onInput"
                @focus="$emit('focus')"
                @blur="$emit('blur')"
            ></b-timepicker>
            <input-error-message v-if="!hideErrors" :text="errors[0]" />
        </validation-provider>
    </b-field>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';

@Component({
    name: 'input-timepicker'
})
export default class InputTimepicker extends Vue {
    @Prop({ default: null })
    id!: string | null;

    @Prop()
    label!: string;

    @Prop()
    required!: boolean;

    @Prop()
    rules!: string;

    @Prop()
    value!: Date;

    @Prop({ default: false })
    disabled!: boolean;

    @Prop({ default: false })
    hideErrors!: boolean;

    @Prop({ default: false })
    hideLabel!: boolean;

    @Prop()
    placeholder!: string | null;

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

    setError(error: string) {
        (this.$refs.validator as any).applyResult({
            errors: [error],
            valid: false,
            failedRules: {}
        });
    }
}
</script>",