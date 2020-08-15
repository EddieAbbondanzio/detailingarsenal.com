<template>
    <b-modal :active.sync="isActive" :has-modal-card="true">
        <div class="modal-card">
            <div
                class="modal-card-head has-background-info is-radiusless is-flex is-flex-row is-justify-content-space-between"
            >
                <div class="is-flex is-flex-column">
                    <p class="has-text-light is-size-5">{{ year }}</p>
                    <p class="has-text-white is-size-3">{{ day }}</p>
                </div>

                <b-button
                    icon-left="calendar-today"
                    type="is-text"
                    class="has-text-white"
                    size="is-large"
                    title="Return to today"
                    @click="date = new Date()"
                />
            </div>
            <div class="modal-card-body is-flex is-flex-column is-align-items-center">
                <b-datepicker v-model="date" :inline="true" />
            </div>

            <div class="modal-card-foot has-background-white has-border-top-0 is-radiusless">
                <b-button type="is-primary" @click="setDate()">Done</b-button>
                <b-button type="is-light" @click="isActive = false">Cancel</b-button>
            </div>
        </div>
    </b-modal>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import moment from 'moment';
import calendarStore from '../store/calendar-store';

@Component({
    name: 'calendar-datepicker-modal'
})
export default class CalendarDatepickerModal extends Vue {
    get year() {
        return moment(this.date).format('YYYY');
    }

    get day() {
        return moment(this.date).format('ddd, MMMM Do');
    }

    isActive: boolean = false;
    date: Date = new Date();

    show() {
        this.date = calendarStore.date;

        this.isActive = true;
    }

    setDate() {
        calendarStore.jumpDate(this.date);
        this.isActive = false;
    }
}
</script>