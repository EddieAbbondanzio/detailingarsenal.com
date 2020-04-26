<template>
    <div :class="classes()" :style="styles()" @mousedown.left.stop="$emit('moveStart', $event)">
        <a
            class="action"
            @click="$emit('delete', $event)"
            @mousedown.stop
            v-if="value.meta.pending"
        ></a>

        <!-- Appointment Info -->
        <div class="block-content is-flex is-flex-column is-size-7">
            <span class="has-margin-right-1">{{ name }}</span>
            <span>{{ start }} - {{ end }}</span>
        </div>

        <div class="block-resizer" @mousedown.left.stop="$emit('resizeStart', $event)">&nbsp;</div>
    </div>
</template>

<style lang="sass" scoped>
.block
    .action
        position: absolute
        top: 4px
        right: 4px   

    .block-resizer
        cursor: ns-resize
        height: 8px
        position: absolute
        left: 0px
        right: 0px
        bottom: 0px
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { AppointmentBlock } from '../../api/calendar/entities/appointment-block';
import settingsStore from '../../store/settings/settings-store';
import calendarStore from '../../store/calendar/calendar-store';
import moment from 'moment';

@Component({
    name: 'calendar-block'
})
export default class CalendarBlock extends Vue {
    @Prop()
    value!: AppointmentBlock;

    get name() {
        if (this.value.appointment != null) {
            return settingsStore.services.find(s => s.id == this.value.appointment.serviceId)!.name;
        } else {
            return '(No service specified)';
        }
    }

    get start() {
        return moment(this.value.start).format('hh:mm a');
    }

    get end() {
        return moment(this.value.end).format('hh:mm a');
    }

    styles() {
        return {
            height: `${(this.value.duration / 15) * 20}px`,
            cursor: 'grab',
            'pointer-events': calendarStore.modifyingBlock == null ? 'auto' : 'none' // Don't touch.
        };
    }

    classes() {
        const isCompact = this.value.duration <= 15;

        return {
            'is-flex': true,
            'is-flex-column': !isCompact,
            'has-padding-x-2': true,
            'has-padding-y-2': this.value.duration > 30,
            'is-flex-row': isCompact,
            block: true,
            'is-modifying': this.value.meta.modifying,
            'is-pending': this.value.meta.pending && !this.value.meta.modifying
        };
    }
}
</script>