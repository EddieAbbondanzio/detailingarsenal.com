<template>
    <b-field :label="hideLabel ? null : label" :class="required ? 'is-required' : ''">
        <template v-slot:label>
            <slot name="label"></slot>
        </template>

        <validation-provider :vid="vid" :name="label" :rules="rules" v-slot="{ errors, classes }" ref="validator">
            <b-taginput
                :value="value"
                @input="onInput"
                :class="classes"
                :disabled="disabled"
                :autocomplete="autocomplete"
                :data="data"
            >
                <slot></slot>
            </b-taginput>
            <input-error-message v-if="!hideErrors" :text="errors[0]" />
        </validation-provider>
    </b-field>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'input-tagger'
})
export default class InputTagger extends Vue {
    @Prop({ default: null })
    id!: string | null;

    @Prop()
    label!: string;

    @Prop()
    required!: boolean;

    @Prop()
    rules!: string;

    @Prop()
    value!: any[];

    @Prop({ default: false })
    disabled!: boolean;

    @Prop({ default: false })
    hideErrors!: boolean;

    @Prop({ default: false })
    hideLabel!: boolean;

    @Prop({ default: false })
    autocomplete!: boolean;

    @Prop({ default: () => [] })
    data!: any[];

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
</script>