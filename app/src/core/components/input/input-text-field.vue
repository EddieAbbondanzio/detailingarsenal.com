<template>
    <b-field :label="hideLabel ? null : label" :class="required ? 'is-required' : ''">
        <template v-slot:label>
            <slot name="label"></slot>
        </template>

        <validation-provider :vid="vid" :name="label" :rules="rules" v-slot="{ errors, classes }" ref="validator">
            <b-input
                :icon="iconLeft"
                :type="type"
                :class="classes"
                :value="value"
                :placeholder="placeholder"
                :disabled="disabled"
                :maxlength="maxLength"
                @input="onInput"
                @focus="$emit('focus')"
                @blur="$emit('blur')"
            ></b-input>
            <input-error-message v-if="!hideErrors" :text="errors[0]" />
        </validation-provider>
    </b-field>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

/**
 * Text field that supports validation, and can have a required * indicator.
 */
@Component({
    name: 'input-text-field',
})
export default class InputTextField extends Vue {
    /**
     * Hidden name of the field.
     */
    @Prop({ default: null })
    id!: string | null;

    @Prop()
    label!: string;

    @Prop()
    required!: boolean;

    @Prop()
    rules!: string;

    @Prop()
    value!: any;

    @Prop({ default: false })
    disabled!: boolean;

    @Prop({ default: false })
    hideErrors!: boolean;

    @Prop({ default: false })
    hideLabel!: boolean;

    /**
     * Native type. Use 'textarea' for large inputs
     */
    @Prop({ default: 'text' })
    type!: string;

    @Prop()
    iconLeft!: string;

    @Prop()
    placeholder!: string | null;

    @Prop()
    maxLength!: string | null;

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
            failedRules: {},
        });
    }
}
</script>
",
