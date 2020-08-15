<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Hours of operation"
                description="Hours the business is open"
                icon="clock-outline"
                :backButtonTo="{name: 'settings'}"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb
                            name="Hours of operation"
                            :to="{name: 'hoursOfOperation'}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <edit-button :to="{name: 'editHoursOfOperation'}" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless">
            <div
                class="is-flex is-flex-row is-align-items-center"
                v-for="day in days"
                :key="day.name"
            >
                <span
                    class="time-label-short is-hidden-tablet"
                >{{ dayName(day.day).substring(0,3) }}</span>
                <span class="time-label is-hidden-mobile">{{ dayName(day.day) }}</span>
                <div grouped class="is-flex is-flex-row is-align-items-center">
                    <span>{{ day.open | twelveHourFormat }}</span>

                    <span class="has-margin-x-1">to</span>

                    <span>{{ day.close | twelveHourFormat }}</span>
                </div>
            </div>
        </div>
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
import settingsStore from '../../store/settings-store';
import { displayLoading } from '@/core';

@Component({
    name: 'hours-of-operation'
})
export default class HoursOfOperation extends Vue {
    get days() {
        return settingsStore.hoursOfOperation != null ? settingsStore.hoursOfOperation.days : [];
    }

    @displayLoading
    public async created() {
        await settingsStore.init();
    }

    public dayName(index: number) {
        switch (index) {
            case 0:
                return 'Sunday';
            case 1:
                return 'Monday';
            case 2:
                return 'Tuesday';
            case 3:
                return 'Wednesday';
            case 4:
                return 'Thursday';
            case 5:
                return 'Friday';
            case 6:
                return 'Saturday';
        }
    }
}
</script>
