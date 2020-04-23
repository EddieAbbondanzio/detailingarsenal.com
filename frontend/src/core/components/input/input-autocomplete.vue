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
            <b-autocomplete
                :type="type"
                :class="classes"
                :value="value"
                :field="field"
                :data="data"
                :placeholder="placeholder"
                :disabled="disabled"
                :open-on-focus="true"
                @input="onInput"
                @focus="$emit('focus')"
                @blur="$emit('blur')"
            >
                <template v-slot:footer>
                    <slot name="footer"></slot>
                </template>
            </b-autocomplete>
            <input-error-message v-if="!hideErrors" :text="errors[0]" />
        </validation-provider>
    </b-field>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-auto-complete'
})
export default class InputAutoComplete extends Vue {
    @Prop({ default: null })
    id!: string | null;

    @Prop()
    label!: string;

    @Prop()
    required!: boolean;

    @Prop()
    rules!: string;

    @Prop()
    data!: any[];

    @Prop()
    field!: string;

    @Prop()
    value!: any;

    @Prop({ default: false })
    disabled!: boolean;

    @Prop({ default: false })
    hideErrors!: boolean;

    @Prop({ default: false })
    hideLabel!: boolean;

    @Prop({ default: 'text' })
    type!: string;

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