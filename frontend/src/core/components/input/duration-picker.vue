<template>
    <b-timepicker
        class="duration-picker"
        :min-time="min"
        :max-time="max"
        :increment-minutes="15"
        v-model="date"
        :disabled="disabled"
    />
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';

/**
 * Picker to select a duration in hours / minutes. Value is stored
 * as an integer (number of minutes).
 */
@Component({
    name: 'duration-picker'
})
export default class DurationPicker extends Vue {
    /**
     * Min time value. 00:00
     */
    get min() {
        const d = new Date();
        d.setHours(0);
        d.setMinutes(0);

        return d;
    }

    /**
     * Max time value. 12:00
     */
    get max() {
        const d = new Date();
        d.setHours(12);
        d.setMinutes(0);

        return d;
    }

    /**
     * Duration in minutes.
     */
    @Prop({ default: 0 })
    public value!: number;

    @Prop({ default: false })
    public disabled!: boolean;

    /**
     * Underlying date used to interact with the timepicker.
     */
    public date: Date = new Date();

    /**
     * On load, calculate the hours and minutes
     */
    public created() {
        const minutes = this.value % 60;
        const hours = (this.value - minutes) / 60;

        this.date.setHours(hours);
        this.date.setMinutes(minutes);
    }

    @Watch('value')
    public onValueChange() {
        this.date = new Date();

        const minutes = this.value % 60;
        const hours = (this.value - minutes) / 60;

        this.date.setHours(hours);
        this.date.setMinutes(minutes);
    }

    /**
     * Listen for a change from the timepicker so we can
     * update our local v-model.
     */
    @Watch('date')
    public onDateChange() {
        const minutes = this.date.getHours() * 60 + this.date.getMinutes();
        this.$emit('input', minutes);
    }
}
</script>