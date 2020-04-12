<template>
    <b-dropdown position="is-bottom-left" ref="dropdown">
        <template v-slot:trigger>
            <b-button
                type="is-text"
                icon-left="dots-vertical"
                size="is-medium"
                @click.native="show($event)"
            />
        </template>

        <b-dropdown-item
            aria-role="listitem"
            class="is-flex is-flex-row is-align-items-center"
            @click.native="onEdit($event)"
        >
            <b-icon icon="pencil" type="is-dark" class="has-margin-right-2" />Edit
        </b-dropdown-item>
        <b-dropdown-item
            aria-role="listitem"
            class="is-flex is-flex-row is-align-items-center"
            @click.native="onDelete($event)"
        >
            <b-icon icon="delete" type="is-danger" class="has-margin-right-2" />Delete
        </b-dropdown-item>
    </b-dropdown>
</template>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';

@Component({
    name: 'edit-delete-dropdown'
})
export default class EditDeleteDropdown extends Vue {
    @Ref('dropdown')
    public dropdown: any;

    /*
     * We need to prevent events from bubbling to parent so we do things weird.
     */

    show(event: Event) {
        event.preventDefault();
        event.stopPropagation();
        this.dropdown.toggle();
    }

    onEdit(event: Event) {
        event.preventDefault();
        event.stopPropagation();
        this.$emit('edit');
    }

    onDelete(event: Event) {
        event.preventDefault();
        event.stopPropagation();
        this.$emit('delete');
    }
}
</script>