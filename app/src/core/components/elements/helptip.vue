<template>
    <div>
        <b-tooltip :label="label">
            <b-icon icon="account" />
        </b-tooltip>
    </div>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { Prop } from 'vue-property-decorator';

@Component({ name: 'help-tip' })
export default class HelpTip extends Vue {
    @Prop({ default: '' })
    label!: string;
}
</script>
