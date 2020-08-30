<template>
    <progress
        class="progress is-inline-block is-small"
        type="is-dark"
        :value="value"
        :min="0"
        :max="10"
    ></progress>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PadCut } from '@/api';

@Component
export default class PadCutBar extends Vue {
    get type() {
        switch (this.value) {
            case 0:
            case 1:
                return 'is-green';
            case 2:
            case 3:
            case 4:
                return 'is-yellow';
            case 5:
            case 6:
            case 7:
                return 'is-orange';
            case 8:
            case 9:
            case 10:
                return 'is-red';
        }
    }

    /**
     * Pad cut level
     */
    @Prop()
    value!: PadCut;
}
</script>