<template>
    <page>
        <template v-slot:header>
            <page-header :title="value != null ? value.label : ``">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Pads" :to="{ name: 'pads' }" />
                        <breadcrumb
                            :name="value != null ? value.label : ''"
                            :to="{ name: 'pad', params: $route.params }"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless is-flex is-flex-column is-flex-grow-1" v-if="value != null">
            <div class="is-flex is-flex-row-desktop is-flex-column">
                <div class="has-margin-right-3-desktop is-align-self-center is-align-self-start-desktop">
                    <img class="img is-square" src="https://bulma.io/images/placeholders/480x480.png" />
                </div>

                <div class="is-flex-grow-1">
                    <div class="has-margin-bottom-3">
                        <p class="is-size-4 is-size-3-desktop">{{ value.label }}</p>
                        <div class="is-flex is-flex-row">
                            <stars :value="3" :count="20" />
                        </div>

                        <div class="columns">
                            <div class="column is-one-quarter">
                                <p class="is-size-6 has-text-weight-bold">Cut</p>
                                <pad-cut-bar :value="value.cut" />
                            </div>

                            <div class="column is-one-quarter">
                                <p class="is-size-6 has-text-weight-bold">Finish</p>
                                <pad-finish-bar :value="value.finish" />
                            </div>
                        </div>
                    </div>

                    <div class="has-margin-bottom-3">
                        <div class="is-flex is-flex-column is-flex-row-tablet is-justify-content-space-around">
                            <div class="is-flex-grow-1 is-flex-basis-0 has-margin-bottom-3-mobile">
                                <p class="is-size-5 title">Brand</p>
                                <p class="is-size-6 subtitle">{{ value.series.brand.name }}</p>
                            </div>

                            <div class="is-flex-grow-1 is-flex-basis-0 has-margin-bottom-3-mobile">
                                <p class="is-size-5 title">Series</p>
                                <p class="is-size-6 subtitle">{{ value.series.name }}</p>
                            </div>

                            <div class="is-flex-grow-1 is-flex-basis-0">
                                <p class="is-size-5 title">Name</p>
                                <p class="is-size-6 subtitle">{{ value.name }}</p>
                            </div>
                        </div>
                    </div>

                    <div class="has-margin-bottom-3">
                        <div class="is-flex is-flex-column is-flex-row-tablet is-justify-content-space-around">
                            <div class="is-flex-grow-1 is-flex-basis-0 has-margin-bottom-3-mobile">
                                <p class="is-size-5 title">Category</p>
                                <p class="is-size-6 subtitle">{{ value.category | uppercaseFirst }}</p>
                            </div>

                            <div class="is-flex-grow-1 is-flex-basis-0 has-margin-bottom-3-mobile">
                                <p class="is-size-5 title">Material</p>
                                <p class="is-size-6 subtitle">{{ value.material | uppercaseFirst }}</p>
                            </div>

                            <div class="is-flex-grow-1 is-flex-basis-0">
                                <p class="is-size-5 title">Texture</p>
                                <p class="is-size-6 subtitle">{{ value.texture | uppercaseFirst }}</p>
                            </div>
                        </div>
                    </div>

                    <div class="has-margin-bottom-3">
                        <p class="is-size-5 title has-margin-bottom-1">Recommended For Polisher Type(s)</p>
                        <ul>
                            <li v-for="t in value.polisherTypes" :key="t"><polisher-type-tag :value="t" /></li>
                        </ul>
                    </div>

                    <div class="has-margin-bottom-3">
                        <p class="is-size-5 title has-margin-bottom-1">Sizes</p>

                        <b-table :data="sizes">
                            <b-table-column v-slot="props" label="Diameter" field="diameter" sortable>
                                {{ props.row.diameter }}
                            </b-table-column>
                            <b-table-column v-slot="props" label="Thickness" field="thickness" sortable>
                                {{ props.row.thickness }}
                            </b-table-column>
                            <b-table-column v-slot="props" label="Part Number" field="partNumber">
                                {{ props.row.partNumber }}
                            </b-table-column>
                        </b-table>
                    </div>
                </div>
            </div>

            <div>
                <div class="is-flex is-flex-row is-align-items-center has-margin-bottom-2">
                    <p class="is-size-5 title has-margin-bottom-0">Reviews</p>
                    <b-button
                        class="has-margin-left-1"
                        type="is-success"
                        size="is-small"
                        tag="router-link"
                        :to="{ name: 'writeReview' }"
                        >Write a review</b-button
                    >
                </div>

                <div class="has-margin-bottom-2" v-for="(review, i) in reviews" :key="i">
                    <p class="has-text-weight-bold">
                        {{ review.username }}
                        <span class="is-size-7 has-text-weight-normal">{{ review.date | date }}</span>
                    </p>

                    <div class="is-flex is-flex-row">
                        <stars :value="review.stars" :hideCount="true" />

                        <p class="has-text-weight-bold has-margin-left-1">{{ review.title }}</p>
                    </div>

                    <p>{{ review.body }}</p>

                    <div class="is-flex is-flex-row">
                        <div class>
                            <span class="has-margin-right-1 has-text-weight-bold">Cut:</span>
                            <span v-if="review.cut">{{ review.cut }} / 10</span>
                            <span v-else>N/A</span>
                        </div>

                        <div class="has-margin-left-1">
                            <span class="has-margin-right-1 has-text-weight-bold">Finish:</span>
                            <span v-if="review.finish">{{ review.finish }} / 10</span>
                            <span v-else>N/A</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PadColor, PadSeries, PadSize } from '@/api';
import padStore from '../store/pad/pad-store';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import PadCutBar from '@/modules/product-catalog/pads/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/pads/components/pad-finish-bar.vue';
import reviewStore from '../store/review/review-store';
import PolisherTypeTag from '@/modules/shared/components/polisher-type-tag.vue';

@Component({
    components: {
        Stars,
        PadCutBar,
        PadFinishBar,
        PolisherTypeTag,
    },
})
export default class PadView extends Vue {
    get id() {
        return this.$route.params.id;
    }

    get reviews() {
        return reviewStore.reviews;
    }

    value: PadColor | null = null;
    sizes: PadSize[] = [];

    async created() {
        this.value = await padStore.getPadById(this.id);

        if (this.value == null) {
            return;
        }

        this.sizes = this.value.series.sizes;

        reviewStore.loadReviews(this.id);
    }
}
</script>