<template>
    <div class="is-flex is-flex-row is-justify-content-space-between has-margin-bottom-3" v-if="value != null">
        <input-slider
            class="is-flex-grow-1"
            label="Finish"
            :value="value"
            @input="onInput"
            :min="0"
            :max="10"
            :ticks="[
                { value: 0, label: '0 (Worst)' },
                { value: 1, label: 1 },
                { value: 2, label: 2 },
                { value: 3, label: 3 },
                { value: 4, label: 4 },
                { value: 5, label: 5 },
                { value: 6, label: 6 },
                { value: 7, label: 7 },
                { value: 8, label: 8 },
                { value: 9, label: 9 },
                { value: 10, label: '10 (Best)' }
            ]"
        />

        <a class="delete" @click="onDelete"></a>
    </div>
    <div v-else>
        <label class="label">Finish</label>
        <div class="is-flex is-flex-row is-justify-content-space-between">
            Not rated
            <b-button type="is-text" @click="onUndo">Undo</b-button>
        </div>
    </div>
</template>

<script lang="ts">
import { PadFinish } from '@/api/product-catalog';
import { Component, Vue, Prop } from 'vue-property-decorator';

/**
 * Input to allow user to select a level or pad finish, or leave it empty
 * for creating, or updating a review.
 */
@Component({ name: 'pad-finish-input' })
export default class PadFinishInput extends Vue {
    @Prop({ default: 0 })
    value!: PadFinish | null;

    created() {
        this.$emit('input', 0);
    }

    onInput(val: any) {
        this.$emit('input', val);
    }

    onUndo() {
        this.$emit('input', 0);
    }

    onDelete() {
        this.$emit('input', null);
    }
}
</script>
