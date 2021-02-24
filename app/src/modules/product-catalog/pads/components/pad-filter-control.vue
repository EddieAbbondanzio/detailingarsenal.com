<template>
    <div>
        <p class="is-size-5 has-text-weight-bold has-margin-bottom-3">Filter</p>

        <b-field label="Brands">
            <div class="is-flex is-flex-column">
                <input-checkbox label="All" @input="toggleAllBrands" class="has-margin-bottom-0" />
                <input-checkbox
                    :nativeValue="b.id"
                    :label="b.name"
                    v-model="selectedBrands"
                    class="has-margin-bottom-0"
                    v-for="b in brands"
                    :key="b.id"
                />
            </div>
        </b-field>

        <!-- Series -->
        <b-field label="Series">
            <div class="is-flex is-flex-column">
                <input-checkbox label="All" @input="toggleAllSeries" class="has-margin-bottom-0" />
                <input-checkbox
                    :nativeValue="s.id"
                    :label="s.name"
                    v-model="selectedSeries"
                    class="has-margin-bottom-0"
                    v-for="s in series"
                    :key="s.id"
                />
            </div>
        </b-field>

        <!-- Reset -->
        <b-button type="is-danger" outlined @click="onReset">Reset</b-button>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PadCategory, Pad } from '@/api';
import { padCategory } from '@/modules/product-catalog/pads/filters/pad-category';
import { FilterType } from '../store/filter-type';
import { Filter } from '../store/filter';
import store from '@/core/store';
import { MutationPayload } from 'vuex';
import padStore from '../store/pad/pad-store';
import { ArrayUtils } from '@/core/utils/array-utils';

@Component({
    name: 'pad-filter-control',
    filters: {
        padCategory
    }
})
export default class PadFilterControl extends Vue {
    /**
     * Unique list of pad brands
     */
    get brands() {
        return padStore.filter.brands;
    }
    /**
     * Unique list of pad series names
     */
    get series() {
        return padStore.filter.series;
    }
    get categories(): PadCategory[] {
        return Object.values(PadCategory);
    }
    selectedBrands: string[] = [];
    selectedSeries: string[] = [];

    created() {
        this.onReset();
    }

    onInput() {
        // padStore.SET_FILTER(
        //     new Filter(this.selectedBrands, this.selectedSeries, this.selectedCategories as PadCategory[])
        // );
    }
    onReset() {
        this.selectedBrands = [...this.brands.map(b => b.id)];
        this.selectedSeries = [...this.series.map(s => s.id)];
        // padStore.SET_FILTER(
        //     new Filter(this.selectedBrands, this.selectedSeries, this.selectedCategories as PadCategory[])
        // );
    }

    toggleAllBrands(value: boolean) {
        if (value) {
            this.selectedBrands = [...this.brands.map(b => b.id)];
        } else {
            this.selectedBrands = [];
        }
    }

    toggleAllSeries(value: boolean) {
        if (value) {
            this.selectedSeries = [...this.series.map(b => b.id)];
        } else {
            this.selectedSeries = [];
        }
    }
}
</script>
