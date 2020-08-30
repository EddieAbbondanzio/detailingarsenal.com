<template>
    <div class="is-flex is-flex-column is-flex-grow-1 has-overflow-y-hidden">
        <product-catalog-navbar />

        <div class="app-content has-overflow-y-hidden is-flex is-flex-column is-flex-grow-1">
            <page>
                <template v-slot:header>
                    <page-header
                        title="Pad Compare Tool"
                        :description="`Compare buffing pads of multiple brands based on cut level`"
                        :backButton="false"
                    >
                        <template v-slot:action>
                            <b-button type="is-primary" outlined @click="onFiltersClick">Filters</b-button>
                        </template>
                    </page-header>
                </template>

                <template v-slot:sidebar>
                    <page-sidebar ref="sidebar" :overlay="true">
                        <pad-filter-control />
                    </page-sidebar>
                </template>

                <b-table class="pads-table" :data="summaries">
                    <b-table-column
                        v-slot="props"
                    >{{ props.row.image != null ? props.row.image.name : '' }}</b-table-column>
                    <b-table-column
                        v-slot="props"
                        label="Size"
                        field="diameter"
                        sortable
                    >{{ props.row.diameter}}</b-table-column>
                    <b-table-column
                        v-slot="props"
                        label="Brand"
                        field="brand"
                        sortable
                    >{{ props.row.brand }}</b-table-column>
                    <b-table-column
                        v-slot="props"
                        label="Series"
                        field="series"
                        sortable
                    >{{ props.row.series }}</b-table-column>
                    <b-table-column
                        v-slot="props"
                        label="Name"
                        field="name"
                        sortable
                    >{{ props.row.name }}</b-table-column>
                    <b-table-column
                        v-slot="props"
                        label="Category"
                        field="category"
                        sortable
                    >{{ props.row.category |uppercaseFirst }}</b-table-column>
                    <b-table-column v-slot="props" label="Cut" field="cut" width="120px" sortable>
                        <pad-cut-bar :value="props.row.cut" />
                    </b-table-column>
                    <b-table-column
                        v-slot="props"
                        label="Finish"
                        field="finish"
                        width="120px"
                        sortable
                    >
                        <pad-finish-bar :value="props.row.finish" />
                    </b-table-column>
                    <b-table-column
                        v-slot="props"
                        label="Material"
                        field="material"
                        sortable
                    >{{ props.row.material}}</b-table-column>
                    <b-table-column
                        v-slot="props"
                        label="Polisher Type(s)"
                        field="recommendedFor"
                        sortable
                    >
                        <div class="tags">
                            <span
                                class="tag"
                                v-for="rec in props.row.recommendedFor"
                                :key="rec"
                            >{{ rec }}</span>
                        </div>
                    </b-table-column>

                    <template slot="empty">
                        <div class="is-flex is-justify-content-center">There's nothing here!</div>
                    </template>
                </b-table>
            </page>
        </div>
    </div>
</template>

<style lang="sass">
.pads-table
    td
        margin: auto!important
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import ProductCatalogNavbar from '@/modules/product-catalog/components/product-catalog-navbar.vue';
import padStore from '@/modules/product-catalog/store/pad-store';
import { api, Pad, PadCategory } from '@/api';
import { displayLoading } from '@/core';
import PadFilterControl from '@/modules/product-catalog/components/pad-filter-control.vue';
import store from '@/core/store';
import { MutationPayload } from 'vuex';
import PageSidebar from '@/core/components/page/page-sidebar.vue';
import PadCutBar from '@/modules/product-catalog/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/components/pad-finish-bar.vue';

@Component({
    components: {
        ProductCatalogNavbar,
        PadFilterControl,
        PadCutBar,
        PadFinishBar
    }
})
export default class Pads extends Vue {
    columns: Column[] = [];

    unSub!: () => void;

    summaries: PadSummary[] = [
        {
            id: '1',
            name: 'White Polishing',
            brand: 'Lake Country',
            series: 'CCS',
            category: 'polish',
            cut: 5,
            finish: 7,
            diameter: '5"',
            material: 'Foam',
            recommendedFor: ['DA', 'Long Throw']
        },
        {
            id: '2',
            name: 'Orange Light Cutting',
            brand: 'Lake Country',
            series: 'ThinPro',
            category: 'cut',
            cut: 8,
            finish: 7,
            diameter: '5"',
            material: 'Foam',
            recommendedFor: ['DA', 'Long Throw']
        }
    ];

    @displayLoading
    async created() {
        // await padStore.init();
        // this.columns = this.generateColumns();
        // this.unSub = store.subscribe((mut: MutationPayload, state: any) => {
        //     if (mut.type == 'pad/SET_FILTER') {
        //         this.columns = this.generateColumns();
        //     }
        // });
    }

    destroyed() {
        this?.unSub();
    }

    generateColumns(): Column[] {
        const columns: Column[] = [
            { title: 'Heavy Cut', category: 'heavy_cut', pads: [] },
            { title: 'Medium Cut', category: 'medium_cut', pads: [] },
            { title: 'Heavy Polish', category: 'heavy_polish', pads: [] },
            { title: 'Medium Polish', category: 'medium_polish', pads: [] },
            { title: 'Soft Polish', category: 'soft_polish', pads: [] },
            { title: 'Finishing', category: 'finishing', pads: [] }
        ];

        for (let i = 0; i < padStore.filtered.length; i++) {
            const column = columns.find(c => c.category == padStore.filtered[i].category);

            if (column == null) {
                continue;
            }

            column.pads.push(padStore.filtered[i]);
        }

        return columns;
    }

    onFiltersClick() {
        (this.$refs.sidebar as PageSidebar).open();
    }
}

type Column = { title: string; category: PadCategory; pads: Pad[] };
type PadSummary = {
    id: string;
    image?: { name: string; data: string };
    brand: string;
    name: string;
    series: string;
    category: PadCategory;
    cut: number;
    finish: number;
    diameter: string;
    material: string;
    recommendedFor: string[];
};
</script>