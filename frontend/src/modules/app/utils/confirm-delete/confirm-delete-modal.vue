<template>
    <div class="modal-card">
        <div class="modal-card-head has-padding-all-3 is-flex is-flex-row is-align-items-center">
            <b-icon class="has-padding-right-3" icon="delete" type="is-danger" size="is-large" />

            <div class="is-flex is-flex-column">
                <h1 class="title is-size-3">{{ title }}</h1>
                <p class="subtitle is-size-5">{{ description }}</p>
            </div>
        </div>

        <div class="modal-card-body has-padding-all-3">
            <b-field grouped>
                <div class="control">
                    <b-button type="is-danger" @click="$emit('confirm')">Confirm</b-button>
                </div>
                <div class="control">
                    <b-button type="is-light" @click="$emit('cancel')">Cancel</b-button>
                </div>
            </b-field>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

@Component({
    name: 'confirm-delete-modal'
})
export default class ConfirmDeleteModal extends Vue {
    @Prop({ required: true })
    entityType!: string;

    @Prop({ required: true })
    identifier!: string;

    get title() {
        return `Delete ${this.entityType}`;
    }

    get description() {
        return `Are you sure you want to delete the ${this.entityType} ${this.identifier}`;
    }
}
</script>