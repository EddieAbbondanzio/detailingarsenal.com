<template>
    <div :class="classes" :style="styles">
        <div class="is-flex is-flex-column is-size-7" style="flex-wrap: wrap">
            <span class="has-margin-right-1">{{ name }}</span>
            <span>{{ start | twelveHourFormat }} - {{ end | twelveHourFormat }}</span>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { AppointmentBlock } from '../../api/calendar/entities/appointment-block';
import { getModule } from 'vuex-module-decorators';
import SettingsStore from '../../store/settings/settings-store';

@Component({
    name: 'calendar-block'
})
export default class CalendarBlock extends Vue {
    @Prop()
    value!: AppointmentBlock;

    get name() {
        if (this.value.appointment != null) {
            const settingsStore = getModule(SettingsStore, this.$store);
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