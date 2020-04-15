<template>
    <div :class="classes" :style="styles">FUCK IT</div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { AppointmentBlock } from '../../api/calendar/entities/appointment-block';

@Component({
    name: 'calendar-block'
})
export default class CalendarBlock extends Vue {
    @Prop()
    value!: AppointmentBlock;

    get styles() {
        return {
            height: `${(this.value.duration / 15) * 20}px`
        };
    }

    get classes() {
        return {
            block: true,
            'is-resizing': this.value.meta.resizing
        };
    }

    onDragStart() {}

    onDragEnd() {}
}
</script>