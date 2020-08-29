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

                <!-- Cut-o-Meter -->
                <div
                    class="is-flex is-flex-column is-align-items-center has-margin-bottom-3 is-hidden-mobile"
                >
                    <p class="title">Cut Level</p>
                    <div class="gradient-bar has-w-100">&nbsp;</div>
                    <div class="level has-w-100 is-mobile">
                        <div class="level-left is-size-5">Most</div>
                        <div class="level-right is-size-5">Least</div>
                    </div>
                </div>

                <!-- Categories -->
                <div class="columns">
                    <div class="column" v-for="column in columns" :key="column.category">
                        <p
                            class="is-size-5 has-text-centered has-margin-bottom-3"
                        >{{ column.title }}</p>

                        <!-- Pads -->
                        <div>
                            <div
                                class="card has-margin-bottom-2"
                                v-for="pad in column.pads"
                                :key="pad.id"
                            >
                                <div class="card-image">
                                    <figure class="image is-4by3">
                                        <img
                                            :src="pad.image != null ? pad.image.data : 'https://bulma.io/images/placeholders/1280x960.png'"
                                            alt="Placeholder image"
                                        />
                                    </figure>
                                </div>

                                <div class="card-content">
                                    <p>{{ pad.series.brand.name }}</p>
                                    <p>{{ pad.series.name }}</p>
                                    <p>{{ pad.name }}</p>
                                    <p>{{ pad.color }}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </page>
        </div>
    </div>
</template>

<style lang="sass" scoped>
.gradient-bar
    background: rgb(2,0,36)
    background: linear-gradient(90deg, rgba(2,0,36,1) 0%, rgba(255,0,0,1) 0%, rgba(255,246,0,1) 50%, rgba(0,212,255,1) 100%)
    height: 20px
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

@Component({
    components: {
        ProductCatalogNavbar,
        PadFilterControl
    }
})
export default class Pads extends Vue {
    columns: Column[] = [];

    unSub!: () => void;

    @displayLoading
    async created() {
        await padStore.init();
        this.columns = this.generateColumns();

        this.unSub = store.subscribe((mut: MutationPayload, state: any) => {
            if (mut.type == 'pad/SET_FILTER') {
                this.columns = this.generateColumns();
            }
        });
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
</script>