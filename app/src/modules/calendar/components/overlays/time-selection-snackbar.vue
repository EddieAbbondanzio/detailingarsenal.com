<template>
    <div
        class="time-selection-snackbar has-padding-all-3 has-margin-bottom-4-mobile is-flex is-flex-row is-justify-content-space-between is-align-items-center"
    >
        <span>{{ timeSelected | duration }} selected</span>

        <div>
            <b-button class="has-margin-right-1" type="is-primary" @click="onContinue">Continue</b-button>
            <b-button class="has-margin-left-1" type="is-light" @click="onCancel">Cancel</b-button>
        </div>
    </div>
</template>

<style lang="sass" scoped>
.time-selection-snackbar
    background-color: $dark
    color: $white
    position: absolute
    align-self: center
    bottom: 32px
    max-width: 600px
    min-width: 350px
    z-index: 40
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import calendarStore from '../../store/calendar-store';

@Component({
    name: 'time-selection-snackbar'
})
export default class TimeSelectionSnackbar extends Vue {
    get timeSelected() {
        return calendarStore.pendingBlocks.map(p => p.duration).reduce((a, b) => a + b, 0);
    }

    onContinue() {
        calendarStore.SET_CREATE_STEP('details');
    }

    onCancel() {
        calendarStore.cancelPendingChanges();
    }
}
</script>