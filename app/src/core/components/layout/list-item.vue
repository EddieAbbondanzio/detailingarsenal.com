<template>
    <li class="is-hoverable-light is-flex is-flex-row is-flex-grow-1" :style="styles">
        <component
            :is="to != null ? 'router-link' : 'div'"
            :to="to"
            class="has-padding-all-4-tablet has-padding-all-3 is-flex is-flex-row is-flex-grow-1 is-justify-content-space-between"
        >
            <div class="is-flex is-flex-row">
                <!-- Large Icon -->
                <div
                    class="is-flex is-flex-column is-justify-content-center has-margin-right-3"
                    v-if="$slots['icon'] != null"
                >
                    <slot name="icon"></slot>
                </div>

                <!-- Title, and description -->
                <div>
                    <h3
                        class="is-size-4-tablet is-size-5-mobile has-margin-y-0 has-text-dark has-text-weight-bold"
                    >{{ title }}</h3>
                    <p
                        class="is-size-6-tablet is-size-7-mobile has-text-dark"
                        v-if="description != null"
                    >{{ description }}</p>

                    <slot></slot>
                </div>
            </div>

            <!-- Action buttons -->
            <slot name="actions">
                <div class="is-flex is-flex-column is-align-items-center is-justify-content-center">
                    <b-icon v-if="to != null" icon="chevron-right" type="is-dark" />
                </div>
            </slot>
        </component>
    </li>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { RawLocation } from 'vue-router';

@Component({
    name: 'list-item'
})
export default class ListItem extends Vue {
    @Prop({ required: true })
    title!: string;

    @Prop()
    description!: string;

    @Prop()
    to!: RawLocation;

    @Prop()
    height!: string | null;

    get styles() {
        if (this.height != null) {
            return { height: `${this.height}` };
        }

        return '';
    }
}
</script>