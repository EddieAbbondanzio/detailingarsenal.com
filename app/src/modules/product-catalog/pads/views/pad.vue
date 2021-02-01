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
                    style="max-width: 50%"
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
                            <div class="is-flex is-flex-row">
                                <stars :value="value.rating.stars" :count="value.rating.reviewCount" />
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
                            {{ props.row.partNumber }}
                        </b-table-column>
                    </b-table>
                </div>

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
import { Pad, PadOption, PadSeries, PadSize, PolisherType } from '@/api';
import padStore from '../store/pad/pad-store';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import PadCutBar from '@/modules/product-catalog/pads/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/pads/components/pad-finish-bar.vue';
import reviewStore from '../store/review/review-store';
import PolisherTypeTag from '@/modules/shared/components/polisher-type-tag.vue';
import { measurement } from '@/modules/shared/filters/measurement';
import { uppercaseFirst } from '@/core/filters/uppercase-first';

@Component({
    components: {
        Stars,
        PadCutBar,
        PadFinishBar,
        PolisherTypeTag
    },
    filters: {
        measurement
    }
})
export default class PadView extends Vue {
    get id() {
        return this.$route.params.id;
    }

    get title() {
        if (this.value == null) {
            return '';
        }

        return `${this.value.series.name} ${this.value.name}`;
    }

    get description() {
        return `By ${this.value?.series.brand.name}`;
    }

    get polisherTypes() {
        return this.value?.series.polisherTypes;
    }

    get reviews() {
        return reviewStore.reviews;
    }

    get specs(): { label: string; value: string }[] {
        return [
            { label: 'Manufacturer', value: this.value?.series.brand.name! },
            { label: 'Series', value: this.value?.series.name! },
            { label: 'Category', value: uppercaseFirst(this.value?.category!)! },
            { label: 'Material', value: uppercaseFirst(this.value?.material!) ?? 'N/A' },
            { label: 'Texture', value: uppercaseFirst(this.value?.texture!) ?? 'N/A' },
            { label: 'Color', value: uppercaseFirst(this.value?.color!) ?? 'N/A' },
            { label: 'Center hole', value: this.value?.hasCenterHole ? 'Yes' : 'No' }
        ];
    }

    value: Pad | null = null;
    sizes: PadOption[] = [];

    async created() {
        this.value = await padStore.getPadById(this.id);
        console.log(this.value?.series);

        if (this.value == null) {
            return;
        }

        this.sizes = this.value.options;

        reviewStore.loadReviews(this.id);
    }

    isThin(size: PadSize) {
        return PadSize.isThin(size);
    }
}
</script>
