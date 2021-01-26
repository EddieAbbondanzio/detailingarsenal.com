<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Buffing pad catalog"
                :description="`Compare buffing pads of multiple brands based on cut, finish, size, etc...`"
                :backButton="false"
            >
                <!-- <template v-slot:action>
                            <b-button type="is-primary" outlined @click="onFiltersClick">Filters</b-button>
                </template>-->
            </page-header>
        </template>
        <b-table class="pads-table" :data="summaries" :loading="loading">
            <b-table-column v-slot="props">
                <router-link
                    :to="{
                        name: 'pad',
                        params: { id: props.row.id }
                    }"
                >
                    <img
                        :src="
                            props.row.thumbnailUrl != null
                                ? props.row.thumbnailUrl
                                : 'https://via.placeholder.com/60x60?text=NA'
                        "
                        class="pad-thumb"
                    />
                </router-link>
            </b-table-column>
            <b-table-column v-slot="props" label="Name" field="label" sortable>
                <router-link
                    class="label-link has-text-weight-bold"
                    :to="{
                        name: 'pad',
                        params: { id: props.row.id }
                    }"
                    >{{ props.row.name }}
                    <b-tag type="is-info" v-if="props.row.isThin" size="is-small">Thin</b-tag>
                </router-link>
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
.pad-thumb
    max-height: 40px!important

.label-link
    color: $dark!important

    &:hover
        color: $primary!important

.pads-table
    td
        margin: auto!important
        vertical-align: middle
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { api, PadCategory, PadCut, PadFinish, PadMaterial, PadSize, PolisherType, Rating } from '@/api';
import { displayLoading } from '@/core';
import PadFilterControl from '@/modules/product-catalog/pads/components/pad-filter-control.vue';
import store from '@/core/store';
import { MutationPayload } from 'vuex';
import PageSidebar from '@/core/components/page/page-sidebar.vue';
import PadCutBar from '@/modules/product-catalog/pads/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/pads/components/pad-finish-bar.vue';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import padStore from '../store/pad/pad-store';
import PolisherTypeTag from '@/modules/shared/components/polisher-type-tag.vue';
import appStore from '@/core/store/app-store';

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
                thumbnailUrl: pad.thumbnailUrl,
                name: pad.label,
                category: pad.category,
                material: PadMaterial.Foam,
                cut: pad.cut,
                finish: pad.finish,
                rating: pad.rating,
                polisherTypes: pad.series.polisherTypes,
                isThin: PadSize.isThin(pad.series.sizes[0])
            });
        }

        return summaries;
    }

    loading = false; // used for b-table

    @displayLoading
    async created() {
        this.loading = true;
        await padStore.init();
        this.loading = false;
    }
}

interface PadSummary {
    id: string;
    thumbnailUrl: string | null;
    name: string;
    category: PadCategory;
    material: PadMaterial;
    cut: PadCut | null;
    finish: PadFinish | null;
    rating: Rating;
    polisherTypes: PolisherType[];
    isThin: boolean;
}
</script>
