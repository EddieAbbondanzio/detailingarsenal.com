<template>
    <div>
        <div class="is-flex is-flex-row is-align-items-center has-margin-bottom-3">
            <a @click="onHideSidebar" class="is-hidden-tablet is-flex is-align-items-center">
                <b-icon icon="arrow-left" />
            </a>

            <p class="is-size-5 has-text-weight-bold has-margin-bottom-0">Filter</p>
        </div>

        <pad-filter-control-section label="Brand">
            <div class="is-flex is-flex-column">
                <input-checkbox label="All" @input="toggleAllBrands" class="has-margin-bottom-0" v-model="allBrands" />
                <input-checkbox
                    :nativeValue="b.id"
                    :label="b.name"
                    v-model="selectedBrands"
                    class="has-margin-bottom-0"
                    v-for="b in brands"
                    :key="b.id"
                    @input="onBrandInput"
                />
            </div>
        </pad-filter-control-section>

        <pad-filter-control-section label="Series">
            <div class="is-flex is-flex-column">
                <input-checkbox label="All" @input="toggleAllSeries" class="has-margin-bottom-0" v-model="allSeries" />
                <input-checkbox
                    :nativeValue="s.id"
                    :label="s.name"
                    v-model="selectedSeries"
                    class="has-margin-bottom-0"
                    v-for="s in series"
                    :key="s.id"
                    @input="onSeriesInput"
                    :title="s.brandName"
                />
            </div>
        </pad-filter-control-section>

        <!-- Reset -->
        <b-button type="is-danger" outlined @click="onReset">Reset</b-button>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import padStore from '../store/pad-store';
import { displayLoading } from '@/core';
import PadFilterControlSection from '@/modules/product-catalog/pads/components/pad-filter-control-section.vue';
import appStore from '@/core/store/app-store';

@Component({
    name: 'pad-filter-control',
    components: {
        PadFilterControlSection
    }
})
export default class PadFilterControl extends Vue {
    /**
     * Unique list of pad brands
     */
    get brands() {
        return padStore.legend.brands;
    }
    /**
     * Unique list of pad series names
     */
    get series() {
        return padStore.legend.series;
    }

    selectedBrands: string[] = [];
    selectedSeries: string[] = [];

    allBrands = true;
    allSeries = true;

    async created() {
        await this.refreshData();
    }

    @displayLoading
    async refreshData() {
        // Wide open
        if (this.allBrands && this.allSeries) {
            await padStore.getAll();
        } else {
            await padStore.getAll({
                brands: this.selectedBrands,
                series: this.selectedSeries
            });
        }
    }

    onBrandInput() {
        this.allBrands = false;

        if (this.selectedBrands.length == 0) {
            this.allBrands = true;
        }

        this.refreshData();
    }

    onSeriesInput() {
        this.allSeries = false;

        if (this.selectedSeries.length == 0) {
            this.allSeries = true;
        }

        this.refreshData();
    }

    onReset() {
        this.allBrands = true;
        this.allSeries = true;
        this.selectedBrands = [];
        this.selectedSeries = [];
        // padStore.SET_FILTER(
        //     new Filter(this.selectedBrands, this.selectedSeries, this.selectedCategories as PadCategory[])
        // );
    }

    toggleAllBrands(value: boolean) {
        this.$nextTick(() => {
            this.allBrands = true;
        });

        this.selectedBrands = [];
        this.refreshData();
    }

    toggleAllSeries(value: boolean) {
        if (value) this.allSeries = true;

        this.selectedSeries = [];
        this.refreshData();
    }

    onHideSidebar() {
        appStore.HIDE_SIDEBAR();
    }
}
</script>
