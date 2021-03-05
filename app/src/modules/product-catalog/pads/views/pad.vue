<template>
    <page>
        <template v-slot:header>
            <page-header :title="title" :description="description">
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
                <div
                    class="has-margin-right-3-desktop is-align-self-center is-align-self-start-desktop"
                    style="max-width: 500px"
                >
                    <img
                        class="img is-square"
                        :src="
                            value.image != null ? value.imageUrl : 'https://via.placeholder.com/900x671?text=No+Image'
                        "
                    />
                </div>

                <div class="is-flex-grow-1">
                    <div class="has-margin-bottom-3">
                        <!-- Name + Rating -->
                        <div class="has-margin-bottom-3">
                            <p class="is-size-4 is-size-3-desktop">{{ value.label }}</p>
                            <div class="is-flex is-flex-row is-align-items-center">
                                <stars
                                    class="has-margin-right-1"
                                    :value="value.rating.stars"
                                    :count="value.rating.reviewCount"
                                    :readOnly="true"
                                />

                                <div class="has-margin-right-1">
                                    <span class="has-margin-right-1 has-text-weight-bold">Cut:</span>
                                    <span v-if="value.cut">{{ value.cut }} / 10</span>
                                    <span v-else>N/A</span>
                                </div>

                                <div class="has-margin-left-1">
                                    <span class="has-margin-right-1 has-text-weight-bold">Finish:</span>
                                    <span v-if="value.finish">{{ value.finish }} / 10</span>
                                    <span v-else>N/A</span>
                                </div>
                            </div>
                        </div>

                        <!-- Specs -->
                        <div class="columns">
                            <div class="column is-4 has-text-weight-bold">
                                <p class="has-margin-bottom-1" v-for="(spec, i) in specs" :key="i">{{ spec.label }}</p>
                            </div>
                            <div class="column">
                                <p class="has-margin-bottom-1" v-for="(spec, i) in specs" :key="i">{{ spec.value }}</p>
                            </div>
                        </div>
                    </div>

                    <div class="has-margin-bottom-3">
                        <p class="has-text-weight-bold has-margin-bottom-1">Recommended For Polisher Type(s)</p>

                        <b-taglist>
                            <polisher-type-tag v-for="t in polisherTypes" :key="t" :value="t" />
                        </b-taglist>
                    </div>
                </div>
            </div>

            <div>
                <div class="has-margin-bottom-3">
                    <p class="is-size-5 title has-margin-bottom-1">Sizes</p>

                    <b-table :data="sizes">
                        <b-table-column v-slot="props" label="Diameter" field="diameter" sortable>
                            {{ props.row.diameter | measurement }}
                        </b-table-column>
                        <b-table-column v-slot="props" label="Thickness" field="thickness" sortable>
                            {{ props.row.thickness | measurement }}
                            <b-tag type="is-info" v-if="isThin(props.row)" size="is-small">Thin</b-tag>
                        </b-table-column>
                        <b-table-column v-slot="props" label="Part Number" field="partNumber">
                            <ul>
                                <li v-for="pn in props.row.partNumbers" :key="pn.value">
                                    <span>{{ pn.value }}</span
                                    ><span class="has-text-grey" v-if="pn.notes">({{ pn.notes }})</span>
                                </li>
                            </ul>
                        </b-table-column>
                    </b-table>
                </div>

                <div>
                    <div class="columns">
                        <div class="column is-4">
                            <div class="is-flex is-flex-row is-align-items-center has-margin-bottom-3">
                                <p class="title is-size-4 has-margin-bottom-0 has-margin-right-3">Reviews</p>
                                <b-button
                                    class="has-margin-left-1"
                                    type="is-success"
                                    size="is-small"
                                    tag="router-link"
                                    :to="{ name: 'writeReview' }"
                                    >Write a review</b-button
                                >
                            </div>
                            <rating-stats v-model="value.rating" />
                        </div>

                        <div class="column">
                            <div>
                                <div class="has-margin-bottom-2" v-for="(review, i) in reviews" :key="i">
                                    <p class="has-text-weight-bold">
                                        {{ review.username }}
                                        <span class="is-size-7 has-text-weight-normal">{{ review.date | date }}</span>
                                    </p>

                                    <div class="is-flex is-flex-row">
                                        <stars :readOnly="true" :value="review.stars" :hideCount="true" />

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
                                <!-- Empty -->
                                <div v-if="reviews.length == 0">
                                    Nobody has left a review yet. Get some mad street cred with your friends and be the
                                    first!
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import padStore from '../store/pad/pad-store';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import PadCutBar from '@/modules/product-catalog/pads/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/pads/components/pad-finish-bar.vue';
import reviewStore from '../store/review/review-store';
import PolisherTypeTag from '@/modules/shared/components/polisher-type-tag.vue';
import { measurement } from '@/modules/shared/filters/measurement';
import { uppercaseFirst } from '@/core/filters/uppercase-first';
import RatingStats from '@/modules/product-catalog/core/components/rating-stats.vue';
import { displayLoading } from '@/core';
import { commaSeperate } from '@/core/filters/comma-seperate';
import { Pad } from '@/api/product-catalog/data-transfer-objects/pad';

@Component({
    components: {
        Stars,
        PadCutBar,
        PadFinishBar,
        PolisherTypeTag,
        RatingStats
    },
    filters: {
        measurement
    }
})
export default class PadView extends Vue {
    get padId() {
        return this.$route.params.padId;
    }

    get padSeriesId() {
        return this.$route.params.padSeriesId;
    }

    get title() {
        if (this.value == null) {
            return '';
        }

        return `${this.value?.series?.name ?? ''} ${this.value.name}`;
    }

    get description() {
        return `By ${this.value?.brand.name ?? ''}`;
    }

    get polisherTypes() {
        return this.value?.polisherTypes;
    }

    get reviews() {
        return reviewStore.reviews;
    }

    get specs(): { label: string; value: string }[] {
        return [
            { label: 'Manufacturer', value: this.value?.brand.name! },
            { label: 'Series', value: this.value?.series.name! },
            { label: 'Category', value: commaSeperate(uppercaseFirst(this.value?.category!)) },
            { label: 'Material', value: uppercaseFirst(this.value?.material!) ?? 'N/A' },
            { label: 'Texture', value: uppercaseFirst(this.value?.texture!) ?? 'N/A' },
            { label: 'Color', value: uppercaseFirst(this.value?.color!) ?? 'N/A' },
            { label: 'Center hole', value: this.value?.hasCenterHole ? 'Yes' : 'No' }
        ];
    }

    value: Pad | null = null;
    sizes: any[] = [];

    @displayLoading
    async created() {
        this.value = padStore.pads.values.find(p => p.id == this.padId)!;

        // Only fetch pad if we can't find it
        if (this.value == null) {
            await padStore.get(this.padSeriesId);
            this.value = padStore.pads.values.find(p => p.id == this.padId)!;
        }

        // this.sizes = this.value.options.map(o => {
        //     var size = this.value?.series.sizes.find(s => s.id == o.padSizeId)!;
        //     return {
        //         diameter: size.diameter,
        //         thickness: size.thickness,
        //         partNumbers: o.partNumbers
        //     };
        // });

        reviewStore.loadReviews(this.padId);
    }
}
</script>
