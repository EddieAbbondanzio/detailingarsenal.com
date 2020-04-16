<template>
    <div
        :class="classes"
        :style="styles"
        @mousedown.stop="$emit('mousedown', $event)"
        @mouseup="$emit('mouseup', $event)"
    >
        <div class="block-resizer is-top">&nbsp;</div>

        <!-- Appointment Info -->
        <div class="block-content is-flex is-flex-column is-size-7">
            <span class="has-margin-right-1">{{ name }}</span>
            <span>{{ start | twelveHourFormat }} - {{ end | twelveHourFormat }}</span>
        </div>

        <div class="block-resizer is-bottom">&nbsp;</div>
    </div>
</template>

<style lang="sass" scoped>
.block-content
    cursor: grab

.block-resizer
    cursor: ns-resize
    height: 8px
    position: absolute
    left: 0
    right: 0

    &.is-top
        top: 0

    &.is-bottom
        bottom: 0
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { AppointmentBlock } from '../../api/calendar/entities/appointment-block';
import { getModule } from 'vuex-module-decorators';
import SettingsStore from '../../store/settings/settings-store';
import settingsStore from '../../store/settings/settings-store';

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
        return this.value.time;
    }

    get end() {
        return this.value.time + this.value.duration;
    }

    get styles() {
        return {
            height: `${(this.value.duration / 15) * 20}px`
        };
    }

    get classes() {
        const isCompact = this.value.duration <= 15;

        return {
            'is-flex': true,
            'is-flex-column': !isCompact,
            'has-padding-x-2': true,
            'has-padding-y-2': this.value.duration > 30,
            'is-flex-row': isCompact,
            block: true,
            'is-modifying': this.value.meta.modifying
        };
    }

    onDragStart() {}

    onDragEnd() {}
}
</script>