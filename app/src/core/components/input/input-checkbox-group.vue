<template>
    <b-field :label="label" :class="labelClasses">
        <validation-provider :vid="vid" :name="label" :rules="rules" v-slot="{ errors }" ref="validator">
            <div :class="containerClasses">
                <slot></slot>
            </div>
            <input-error-message v-if="!hideErrors" :text="errors[0]" />
        </validation-provider>
    </b-field>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { Prop } from 'vue-property-decorator';

@Component
export default class InputCheckboxGroup extends Vue {
    get labelClasses() {
        return this.required ? 'is-required' : '';
    }

    get containerClasses() {
        return ['is-flex', this.vertical ? 'is-flex-column' : 'is-flex-row'];
    }

    get vid() {
        if (this.id != null) {
            return this.id;
        } else {
            return this.label;
        }
    }

    @Prop({ default: null })
    id!: string | null;

    @Prop()
    rules!: string;

    @Prop()
    label!: string;

    @Prop()
    required!: boolean;

    @Prop({ default: false })
    vertical!: boolean;

    @Prop({ default: false })
    hideErrors!: boolean;
}
</script>
