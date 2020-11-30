<template>
    <b-tag :type="type">{{ text | uppercaseFirst }}</b-tag>
</template>

<script lang="ts">
import { PolisherType } from '@/api';
import Vue from 'vue';
import Component from 'vue-class-component';
import { Prop } from 'vue-property-decorator';

@Component
export default class PolisherTypeTag extends Vue {
    @Prop()
    value!: PolisherType;

    get type() {
        switch (this.value) {
            case PolisherType.DualAction:
                return 'is-primary';
            case PolisherType.LongThrow:
                return 'is-info';
            case PolisherType.ForcedRotation:
                return 'is-success';
            case PolisherType.Rotary:
                return 'is-warning';
            case PolisherType.Mini:
                return 'is-danger';
        }
    }

    get text() {
        return this.value.replaceAll('_', ' ');
    }
}
</script>