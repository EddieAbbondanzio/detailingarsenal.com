<template>
    <b-field :label="label">
        <div :class="classes">
            <slot></slot>
        </div>
    </b-field>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { Prop } from 'vue-property-decorator';

@Component
export default class InputCheckboxGroup extends Vue {
    get classes() {
        return ['is-flex', this.vertical ? 'is-flex-column' : 'is-flex-row'];
    }

    @Prop()
    label!: string;

    @Prop({ default: false })
    vertical!: boolean;
}
</script>
