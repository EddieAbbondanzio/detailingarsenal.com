<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Edit hours of operation"
                description="Edit the hours the business is open"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb name="Hours of operation" :to="{name: 'hoursOfOperation'}" />
                        <breadcrumb
                            name="Edit"
                            :to="{name: 'editHoursOfOperation'}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form submitText="Save changes" @submit="onSubmit" :errorSummary="true">
            <div
                class="is-flex is-flex-row is-align-items-center"
                v-for="day in days"
                :key="day.name"
            >
                <input-checkbox
                    :id="`enabled-${day.name}`"
                    class="has-margin-bottom-0"
                    v-model="day.enabled"
                    @input="onEnable(day.enabled, day.name)"
                />

                <!-- Day name, or abbreviation -->
                <span class="time-label-short is-hidden-tablet">{{ day.abbreviation }}</span>
                <span class="time-label is-hidden-mobile">{{ day.name }}</span>

                <div class="is-flex is-flex-row is-align-items-center">
                    <input-select
                        :label="`${day.name} open`"
                        :id="`open-${day.name}`"
                        :rules="`requiredIf:@enabled-${day.name}|timeBefore:@close-${day.name}`"
                        class="time-input"
                        v-model="day.open"
                        @input="onTimeInput(day.name)"
                        :hideErrors="true"
                        :hideLabel="true"
                    >
                        <option
                            v-for="time in times"
                            :key="time"
                            :value="time"
                        >{{ time | twelveHourFormat }}</option>
                    </input-select>

                    <span class="has-margin-x-1 has-margin-x-3-tablet">to</span>

                    <input-select
                        :label="`${day.name} close`"
                        :id="`close-${day.name}`"
                        :rules="`requiredIf:@enabled-${day.name}|timeAfter:@open-${day.name}`"
                        class="time-input"
                        v-model="day.close"
                        @input="onTimeInput(day.name)"
                        :hideErrors="true"
                        :hideLabel="true"
                    >
                        <option
                            v-for="time in times"
                            :key="time"
                            :value="time"
                        >{{ time | twelveHourFormat }}</option>
                    </input-select>
                </div>
            </div>
        </input-form>
    </page>
</template>

<style lang="sass" scoped>
.time-label-short
    width: 60px

.time-label
    width: 100px

.time-input
    width: 120px
    margin: 0px!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { ValidationProvider } from 'vee-validate';
import { ValidationObserver } from 'vee-validate';
import { DayOfTheWeek } from '@/core/store/day-of-the-week';
import settingsStore from '../../store/settings-store';
import { TimeUtils, displayError, toast, displayLoading } from '@/core';
import { HoursOfOperationUpdate } from '@/api';

/**
 * View to edit hours of operation.
 */
@Component({
    name: 'edit-hours-of-operation',
    components: {
        ValidationObserver,
        ValidationProvider
    }
})
export default class EditHoursOfOperation extends Vue {
    times: number[] = [];

    public days: { name: DayOfTheWeek; abbreviation: string; enabled: boolean; open?: number; close?: number }[] = [
        { enabled: false, name: 'Sunday', abbreviation: 'Sun' },
        { enabled: false, name: 'Monday', abbreviation: 'Mon' },
        { enabled: false, name: 'Tuesday', abbreviation: 'Tue' },
        { enabled: false, name: 'Wednesday', abbreviation: 'Wed' },
        { enabled: false, name: 'Thursday', abbreviation: 'Thu' },
        { enabled: false, name: 'Friday', abbreviation: 'Fri' },
        { enabled: false, name: 'Saturday', abbreviation: 'Sat' }
    ];

    @displayLoading
    public async created() {
        this.times = [];

        for (let i = 0; i < 48; i++) {
            this.times.push(i * 30);
        }

        const id = Number.parseInt(this.$route.params.id);
        await settingsStore.init();

        const hoursOfOp = settingsStore.hoursOfOperation;

        for (let i = 0; i < this.days.length; i++) {
            const day = hoursOfOp.days.find(d => d.day == TimeUtils.dayNameToIndex(this.days[i].name));

            if (day == null) {
                continue;
            }

            this.days[i].enabled = true;
            this.days[i].open = day.open;
            this.days[i].close = day.close;
        }
    }

    /**
     * If a day is disabled, clear out the open and close time.
     */
    onEnable(enabled: boolean, dayName: string) {
        const day = this.days.find(d => d.name == dayName)!;

        if (!enabled) {
            day.open = null!;
            day.close = null!;
        }
    }

    /**
     * If a open, or close time is selected, and the day is not enabled,
     * enable it.
     */
    onTimeInput(dayName: string) {
        const day = this.days.find(d => d.name == dayName)!;

        if (!day.enabled) {
            day.enabled = true;
        }
    }

    @displayLoading
    public async onSubmit() {
        const hoursOfOp: HoursOfOperationUpdate = { id: settingsStore.hoursOfOperation.id, days: [] };

        for (let i = 0; i < this.days.length; i++) {
            const day = this.days[i];

            // Catch bad data
            if (!day.enabled || day.open == null || day.close == null) {
                continue;
            }
            hoursOfOp.days.push({ day: i, open: day.open, close: day.close, enabled: day.enabled });
        }

        try {
            await settingsStore.updateHoursOfOperation(hoursOfOp);
            toast(`Edited hours of operation`);
            this.$router.push({ name: 'hoursOfOperation' });
        } catch (err) {
            displayError(err);
        }
    }
}
</script>
