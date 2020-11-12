<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Buffing Pad Catalog"
                :description="`Compare buffing pads of multiple brands based on cut, finish, size, etc...`"
                :backButton="false"
            >
                <!-- <template v-slot:action>
                            <b-button type="is-primary" outlined @click="onFiltersClick">Filters</b-button>
                </template>-->
            </page-header>
        </template>
        <b-table class="pads-table" :data="summaries">
            <b-table-column v-slot="props">{{ props.row.image != null ? props.row.image.name : '' }}</b-table-column>
            <b-table-column v-slot="props" label="Name" field="label">
                <router-link
                    class="label-link has-text-weight-bold"
                    :to="{
                        name: 'pad',
                        params: { id: props.row.id }
                    }"
                    >{{ props.row.name }}</router-link
                >
            </b-table-column>
            <b-table-column v-slot="props" label="Category" field="category" sortable>{{
                props.row.category | uppercaseFirst
            }}</b-table-column>
            <b-table-column v-slot="props" label="Material" field="material" sortable>{{
                props.row.material | uppercaseFirst
            }}</b-table-column>
            <b-table-column v-slot="props" label="Cut" field="cut" width="120px" sortable>
                <pad-cut-bar :value="props.row.cut" />
            </b-table-column>
            <b-table-column v-slot="props" label="Finish" field="finish" width="120px" sortable>
                <pad-finish-bar :value="props.row.finish" />
            </b-table-column>
            <b-table-column v-slot="props" label="Rating" field="rating" sortable>
                <stars
                    v-if="props.row.rating != null"
                    :value="props.row.rating.stars"
                    :count="props.row.rating.reviewCount"
                />
                <span class="has-text-grey" v-else>N/A</span>
            </b-table-column>
            <b-table-column v-slot="props" label="Polisher Type(s)" field="polisherTypes" sortable>
                <div class="tags">
                    <polisher-type-tag
                        v-for="rec in props.row.polisherTypes"
                        :key="rec"
                        :value="rec"
                    ></polisher-type-tag>
                </div>
            </b-table-column>

            <template slot="empty">
                <div class="is-flex is-justify-content-center">There's nothing here!</div>
            </template>
        </b-table>
    </page>
</template>

<style lang="sass">
.label-link
    color: $dark!important

    &:hover
        color: $primary!important

.pads-table
    td
        margin: auto!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { api, Pad, PadCategory, PadCut, PadFinish, PadMaterial, PadSeriesSize, PolisherType, Rating } from '@/api';
import { displayLoading } from '@/core';
import PadFilterControl from '@/modules/product-catalog/pads/components/pad-filter-control.vue';
import store from '@/core/store';
import { MutationPayload } from 'vuex';
import PageSidebar from '@/core/components/page/page-sidebar.vue';
import PadCutBar from '@/modules/product-catalog/pads/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/pads/components/pad-finish-bar.vue';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import padStore from '../store/pad/pad-store';
import PolisherTypeTag from '@/modules/product-catalog/pads/components/polisher-type-tag.vue';

@Component({
    components: {
        PadFilterControl,
        PadCutBar,
        PadFinishBar,
        Stars,
        PolisherTypeTag
    }
})
export default class Pads extends Vue {
    get summaries(): PadSummary[] {
        const summaries: PadSummary[] = [];

        for (const pad of padStore.pads) {
            summaries.push({
                id: pad.id,
                name: pad.label,
                category: pad.category,
                material: pad.material,
                cut: pad.cut,
                finish: pad.finish,
                rating: pad.rating,
                polisherTypes: pad.polisherTypes
            });
        }

        return summaries;
    }

    async created() {
        await padStore.init();
    }

    isThin(thickness: number) {
        return thickness < 0.75;
    }
}

interface PadSummary {
    id: string;
    name: string;
    category: PadCategory;
    material: PadMaterial;
    cut: PadCut | null;
    finish: PadFinish | null;
    rating: Rating;
    polisherTypes: PolisherType[];
}
</script>