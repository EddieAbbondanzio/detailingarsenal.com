<template>
    <div class="modal-card">
        <div
            class="modal-card-head has-padding-all-3 is-flex is-flex-row is-align-items-center has-background-danger"
        >
            <b-icon
                class="has-padding-right-3"
                icon="alert-circle"
                type="is-white"
                size="is-large"
            />

            <div class="is-flex is-flex-column">
                <h1 class="title is-size-3 has-text-white">Error</h1>
                <p class="subtitle is-size-5 has-text-light">Something went wrong</p>
            </div>
        </div>

        <div class="modal-card-body has-padding-all-3">
            {{ message }}
            <div class="has-margin-top-3">
                <b-button @click="$emit('close')" type="is-primary">Close</b-button>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'error-modal'
})
export default class ErrorModal extends Vue {
    @Prop({ required: true })
    message!: string;
}
</script>