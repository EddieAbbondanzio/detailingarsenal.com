<template>
    <div class="box is-radiusless has-padding-all-0 has-margin-bottom-0">
        <slot>
            <!-- Mobile -->
            <div class="has-padding-x-3 is-hidden-tablet">
                <nav class="breadcrumb is-hidden-mobile" aria-label="breadcrumbs">
                    <ul>
                        <li
                            v-for="(breadcrumb, index) in breadcrumbs"
                            :key="breadcrumb.name"
                            :class="index == breadcrumbs.length - 1 ? 'is-active' : ''"
                        >
                            <router-link :to="breadcrumb.to">{{ breadcrumb.name }}</router-link>
                        </li>
                    </ul>
                </nav>

                <div class="is-flex is-flex-row is-align-items-center is-justify-content-space-between has-margin-y-2">
                    <div class="is-flex is-flex-row is-align-items-center">
                        <back-button v-if="backButton" :to="backButtonTo" />

                        <div class="has-margin-x-2">
                            <div class="is-flex is-flex-row">
                                <div>
                                    <h1 class="is-size-5-mobile is-size-3 title">{{ title }}</h1>
                                    <p class="is-size-7-mobile is-size-5 subtitle">{{ description }}</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <slot name="action"></slot>
                </div>
            </div>

            <!-- Tablet and up -->
            <div class="is-hidden-mobile section">
                <div class="container">
                    <slot name="breadcrumb-trail"></slot>

                    <div class="is-flex is-flex-row-tablet is-justify-content-space-between-tablet">
                        <div>
                            <div class="is-flex is-flex-row">
                                <div class="is-flex is-flex-column is-justify-content-center" v-if="icon != null">
                                    <b-icon class="has-margin-right-3" :icon="icon" size="is-large" type="is-primary" />
                                </div>
                                <div>
                                    <h1 class="is-size-4-mobile is-size-3 title">{{ title }}</h1>
                                    <p class="is-size-6-mobile is-size-5 subtitle">{{ description }}</p>
                                </div>
                            </div>
                        </div>

                        <slot name="action"></slot>
                    </div>
                </div>
            </div>
        </slot>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import BreadcrumbTrail from './breadcrumb-trail.vue';
import Breadcrumb from './breadcrumb.vue';
import { RawLocation } from 'vue-router';

@Component({
    name: 'page-header'
})
export default class PageHeader extends Vue {
    @Prop()
    icon!: string;

    /**
     * Large bold title in the header.
     */
    @Prop({ default: '' })
    title!: string;

    /**
     * Smaller text below header for page description.
     */
    @Prop({ default: '' })
    description!: string;

    @Prop({ default: () => [] })
    breadcrumbs!: [{ name: string; to: { name: string } | string }];

    @Prop({ default: true })
    backButton!: boolean;

    @Prop()
    backButtonTo!: RawLocation;
}
</script>
