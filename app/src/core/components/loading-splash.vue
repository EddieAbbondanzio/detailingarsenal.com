<template>
    <div
        class="loading-splash is-flex is-flex-column is-flex-grow-1 has-background-primary is-align-items-center is-justify-content-center has-h-50"
    >
        <div class="is-flex is-flex-column is-align-items-center">
            <h1
                class="is-size-3 has-text-white has-font-family-pacifico"
                style="margin-bottom: 128px;"
            >Detailing Arsenal</h1>
            <b-loading :active="true" :is-full-page="false" />
            <p class="has-text-white">{{ message }}</p>
        </div>

        <b-button
            class="is-hidden"
            style="position: absolute; bottom: 12.5%; z-index: 9001"
            type="is-white"
            outlined
            @click="onCancel"
        >Cancel</b-button>
    </div>
</template>

<style lang="sass">
.loading-splash
    .loading-background
        background-color: transparent!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import userStore from '../../modules/scheduling/user/store/user-store';

@Component({
    name: 'loading-splash'
})
export default class LoadingSplash extends Vue {
    message: string = '';
    refreshHandle!: NodeJS.Timeout;

    mounted() {
        this.message = this.pickNextMessage();

        this.refreshHandle = setInterval(() => {
            this.message = this.pickNextMessage();
        }, 1500);
    }

    beforeDestroy() {
        clearInterval(this.refreshHandle);
    }

    pickNextMessage() {
        const rand = Math.floor(Math.random() * loadingMessages.length);
        return loadingMessages[rand];
    }

    onCancel() {
        userStore.logout();
    }
}

export const loadingMessages = [
    'Polishing out swirls',
    'Removing dog hair',
    'Extracting salt stains',
    'Cleaning windows. Again...',
    'Scrubbing bugs',
    'Waxing the paint',
    'Vacuuming up sand and dirt',
    'Removing tree sap',
    'Blasting out wheel wells'
];
</script>