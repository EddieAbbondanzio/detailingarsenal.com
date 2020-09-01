<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Pad Compare Tool"
                :description="`Compare buffing pads of multiple brands based on cut level`"
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
                    :to="{name: 'pad', params: {id: props.row.id}}"
                >{{ props.row.label }}</router-link>
            </b-table-column>
            <b-table-column
                v-slot="props"
                label="Category"
                field="category"
                sortable
            >{{ props.row.category |uppercaseFirst }}</b-table-column>
            <b-table-column
                v-slot="props"
                label="Material"
                field="material"
                sortable
            >{{ props.row.material |uppercaseFirst }}</b-table-column>
            <b-table-column v-slot="props" label="Thickness" field="thickness" sortable>
                {{props.row.thickness | inchify}}
                <span
                    class="tag is-info has-margin-left-1"
                    v-if="isThin(props.row.thickness)"
                >Thin</span>
            </b-table-column>
            <b-table-column v-slot="props" label="Cut" field="cut" width="120px" sortable>
                <pad-cut-bar :value="props.row.cut" />
            </b-table-column>
            <b-table-column v-slot="props" label="Finish" field="finish" width="120px" sortable>
                <pad-finish-bar :value="props.row.finish" />
            </b-table-column>
            <b-table-column v-slot="props" label="Rating" field="stars" sortable>
                <stars :value="props.row.stars" :count="props.row.reviewCount" />
            </b-table-column>
            <b-table-column v-slot="props" label="Polisher Type(s)" field="recommendedFor" sortable>
                <div class="tags">
                    <span class="tag" v-for="rec in props.row.recommendedFor" :key="rec">{{ rec }}</span>
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
import { api, Pad, PadCategory } from '@/api';
import { displayLoading } from '@/core';
import PadFilterControl from '@/modules/product-catalog/components/pad-filter-control.vue';
import store from '@/core/store';
import { MutationPayload } from 'vuex';
import PageSidebar from '@/core/components/page/page-sidebar.vue';
import PadCutBar from '@/modules/product-catalog/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/components/pad-finish-bar.vue';
import padSummaryStore from '@/modules/product-catalog/store/pad-summary/pad-summary-store';
import Stars from '@/modules/product-catalog/components/stars.vue';

@Component({
    components: {
        PadFilterControl,
        PadCutBar,
        PadFinishBar,
        Stars
    }
})
export default class Pads extends Vue {
    get summaries() {
        return padSummaryStore.summaries;
    }

    isThin(thickness: number) {
        return thickness < 0.75;
    }
}
</script>