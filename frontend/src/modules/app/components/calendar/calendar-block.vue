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
        v-mouse:release.left="onMoveEnd"
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
            v-mouse:release.left="onResizeEnd"
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
import { AppointmentBlock, BLOCK_MODIFY_FLAG, BLOCK_MODIFIED } from '../../api/calendar/entities/appointment-block';
import settingsStore from '../../store/settings/settings-store';
import calendarStore from '../../store/calendar/calendar-store';
import moment from 'moment';

@Component({
    name: 'calendar-block'
})
export default class CalendarBlock extends Vue {
    @Prop()
    value!: AppointmentBlock;

    dragOffset = 0;
    state: 'idle' | 'moving' | 'dragging' = 'idle';

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

    onMoveEnd() {
        this.state = 'idle';
        calendarStore.saveBlockChanges(this.value);
    }

    onResizeStart() {
        this.state = 'dragging';
        this.dragOffset = 0;
    }

    onResizeDrag(el: HTMLElement, ev: MouseEvent) {
        this.dragOffset += ev.movementY;

        const intervalsJumped = Math.floor(this.dragOffset / 20);

        if (intervalsJumped) {
            this.resizeBlock(this.value, this.value.time + this.value.duration + intervalsJumped * 15);
            this.dragOffset -= intervalsJumped * 20;
        }
    }

    onResizeEnd() {
        this.state = 'idle';
        calendarStore.saveBlockChanges(this.value);
    }

    onDelete() {
        calendarStore.DELETE_BLOCK(this.value);
    }

    resizeBlock(block: AppointmentBlock, endTime: number) {
        let startTime = 0,
            duration = 0;

        // Easy peazy. We're resizing an existing block.
        if (block.meta.initialTime == null) {
            startTime = block.time;
            duration = endTime - block.time;
        }
        // Down
        else if (block.time < endTime) {
            // Going down, but we went up first
            if (block.meta.initialTime > block.time) {
                startTime = endTime;
                duration = block.meta.initialTime - endTime + 15;
            } else {
                startTime = block.meta.initialTime;
                duration = endTime - block.meta.initialTime + 15;
            }
        }
        // Up
        else {
            startTime = endTime;
            duration = block.meta.initialTime - endTime + 15;
        }

        const start = moment(block.start)
            .startOf('day')
            .add(startTime, 'minutes');

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