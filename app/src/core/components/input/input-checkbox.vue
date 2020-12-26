<template>
    <b-field :class="required ? 'is-required' : ''">
        <template v-slot:label>
            <slot name="label"></slot>
        </template>

        <validation-provider :vid="vid" :name="label" :rules="rules" v-slot="{ errors, classes }" ref="validator">
            <b-checkbox
                :class="classes"
                :value="value"
                @input="onInput"
                :type="type"
                :disabled="disabled"
                :native-value="nativeValue"
            >
                <slot>
                    <span v-if="!hideLabel">{{ label }}</span>
                </slot>
            </b-checkbox>
            <input-error-message v-if="!hideErrors" :text="errors[0]" />
        </validation-provider>
    </b-field>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-checkbox',
})
export default class InputCheckbox extends Vue {
    @Prop({ default: null })
    id!: string | null;

    @Prop()
    label!: boolean;

    @Prop()
    required!: boolean;

    @Prop()
    rules!: string;

    @Prop()
    value!: boolean;

    @Prop({ default: false })
    disabled!: boolean;

    @Prop({ default: false })
    hideErrors!: boolean;

    @Prop({ default: false })
    hideLabel!: boolean;

    @Prop({ default: 'is-primary' })
    type!: string;

    @Prop()
    nativeValue!: string | null;

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