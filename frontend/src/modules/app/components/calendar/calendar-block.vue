<template>
    <!-- <div
        :class="classes()"
        :style="styles()"
        @mousedown.left.stop="$emit('moveStart', $event)"
    >-->
    <div
        :class="classes()"
        :style="styles()"
        v-mouse:hold.left="onMoveStart"
        v-mouse:drag.left="onMoveDrag"
        v-mouse:release.left="onMoveOrResizeEnd"
    >
        <a class="action" @click="onDelete" @mousedown.stop v-if="value.meta.pending"></a>

        <!-- Appointment Info -->
        <div class="block-content is-flex is-flex-column is-size-7">
            <span class="has-margin-right-1">{{ name }}</span>
            <span>{{ start }} - {{ end }}</span>
        </div>

        <div
            class="block-resizer"
            v-mouse:hold.left="onResizeStart"
            v-mouse:drag.left="onResizeDrag"
            v-mouse:release.left="onMoveOrResizeEnd"
            v-mouse:click.left="onMoveOrResizeEnd"
            ref="resize"
        >&nbsp;</div>
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
import { AppointmentBlock, BLOCK_MODIFIED, BLOCK_INITIAL_TIME } from '../../api/calendar/entities/appointment-block';
import settingsStore from '../../store/settings/settings-store';
import calendarStore from '../../store/calendar/calendar-store';
import moment from 'moment';
import { triggerMouseEvent } from '@/core/utils/trigger-mouse-event';

@Component({
    name: 'calendar-block'
})
export default class CalendarBlock extends Vue {
    @Prop()
    value!: AppointmentBlock;

    dragOffset = 0;
    state: 'idle' | 'moving' | 'resizing' = 'idle';

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
            position: 'absolute',
            top: `${(this.value.time / 15) * 20}px`,
            height: `${(this.value.duration / 15) * 20}px`,
            cursor: this.state == 'moving' ? 'grabbing' : 'pointer',
            'pointer-events': 'auto'
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
            'is-modifying': this.state != 'idle',
            'is-pending': this.value.meta.pending && !this.value.meta.modifying
        };
    }

    onMoveStart() {
        this.state = 'moving';
        this.dragOffset = 0;
    }

    onMoveDrag(el: HTMLElement, ev: MouseEvent) {
        this.dragOffset += ev.movementY;

        const intervalsJumped = Math.floor(this.dragOffset / 20);

        if (intervalsJumped) {
            this.moveBlock(this.value, intervalsJumped * 15);
            this.dragOffset -= intervalsJumped * 20;
        }
    }

    onResizeStart() {
        this.state = 'resizing';
        this.dragOffset = 0;
    }

    onResizeDrag(el: HTMLElement, ev: MouseEvent) {
        this.dragOffset += ev.movementY;

        const intervalsJumped = Math.floor(this.dragOffset / 20);

        if (intervalsJumped) {
            this.resizeBlock(this.value, intervalsJumped * 15);
            this.dragOffset -= intervalsJumped * 20;
        }
    }

    onMoveOrResizeEnd() {
        this.state = 'idle';
        calendarStore.saveBlockChanges(this.value);
    }

    onDelete() {
        calendarStore.DELETE_BLOCK(this.value);
    }

    focus() {
        triggerMouseEvent(this.$refs.resize as HTMLElement, 'mousedown');
        calendarStore.ADD_BLOCK_META({ block: this.value, meta: { name: BLOCK_INITIAL_TIME, value: this.value.time } });
    }

    resizeBlock(block: AppointmentBlock, adjustment: number) {
        let startTime = block.time,
            duration = block.duration,
            initialTime = block.meta[BLOCK_INITIAL_TIME];

        // Below
        if (duration + adjustment > 0 && startTime >= initialTime) {
            startTime = initialTime;
            duration += adjustment;
        }
        // Above
        else {
            startTime += adjustment;
            duration = initialTime - startTime;
        }

        const start = moment(block.start)
            .startOf('day')
            .add(startTime, 'minutes');

        // Ensure block is at least 15 minutes long.
        const end = start.clone().add(Math.max(duration, 15), 'minutes');

        const wasBlockModified = !start.isSame(block.start, 'minutes') || !end.isSame(block.end, 'minutes');
        if (wasBlockModified) {
            calendarStore.UPDATE_BLOCK_START({
                block,
                start: start.toDate()
            });

            calendarStore.UPDATE_BLOCK_END({
                block,
                end: end.toDate()
            });

            calendarStore.ADD_BLOCK_META({ block, meta: { name: BLOCK_MODIFIED, value: true } });
        }
    }

    moveBlock(block: AppointmentBlock, changeInMinutes: number) {
        // catch going before midnight (start of day)
        if (block.time + changeInMinutes < 0) {
            changeInMinutes = 0;
        }

        // catch going over midnight (end of day)
        if (block.time + block.duration >= 24 * 4 * 15 && changeInMinutes > 0) {
            changeInMinutes = 0;
        }

        const start = moment(block.start).add(changeInMinutes, 'minutes');
        const end = start.clone().add(block.duration, 'minutes');

        const wasBlockModified = !start.isSame(block.start, 'minutes') || !end.isSame(block.end, 'minutes');

        if (wasBlockModified) {
            calendarStore.UPDATE_BLOCK_START({
                block,
                start: start.toDate()
            });

            calendarStore.UPDATE_BLOCK_END({
                block,
                end: end.toDate()
            });

            calendarStore.ADD_BLOCK_META({ block, meta: { name: BLOCK_MODIFIED, value: true } });
        }
    }
}
</script>