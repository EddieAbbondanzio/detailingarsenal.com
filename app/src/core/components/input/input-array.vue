<template>
    <div class="is-flex is-flex-column">
        <div>
            <b-field :label="title">
                <div class="is-flex is-flex-column">
                    <div v-for="(v, i) in value" :key="i">
                        <div class="is-flex is-flex-row">
                            <slot :value="v"></slot>

                            <b-button
                                class="has-margin-left-2 is-justify-self-center has-margin-top-4"
                                type="is-danger"
                                @click="onDelete(i)"
                                >Delete</b-button
                            >
                        </div>

                        <!-- Nested detail row. Allows for input arrays in input arrays and more -->
                        <div
                            class="is-flex is-flex-row has-padding-left-3 has-border-left-1-light"
                            v-if="!$slots['detail']"
                        >
                            <slot :value="v" name="detail"></slot>
                        </div>
                    </div>

                    <b-button type="is-text" class="is-align-self-start" @click="onAddAnother">Add another</b-button>
                </div>
            </b-field>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';

/**
 * Assists with building an array of an object by providing an "add another" button below the
 * list and a delete button for each item.
 */
@Component({
    name: 'input-array',
})
export default class InputArray extends Vue {
    /**
     * Text atop the array
     */
    @Prop()
    title!: string;

    /**
     * Factory method to create a new instance of the objects
     * being inputted.
     */
    @Prop({ default: () => () => ({}) })
    factory!: () => any;

    /**
     * The array of objects being inputted.
     */
    @Prop()
    value!: any[];

    onAddAnother() {
        this.$emit('input', [...this.value, this.factory()]);
    }

    onDelete(index: number) {
        this.$emit(
            'input',
            this.value.filter((v, i) => i != index)
        );
    }
}
</script>