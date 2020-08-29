<template>
    <div>
        <p class="is-size-5 has-text-weight-bold has-margin-bottom-3">Filter</p>

        <!-- Brand -->
        <div class="has-margin-bottom-2">
            <p class="is-size-6 has-text-weight-bold has-margin-bottom-1">Brand</p>

            <div v-for="brand in brands" :key="brand">
                <b-checkbox
                    :native-value="brand"
                    v-model="selectedBrands"
                    @input.prevent="onInput()"
                >{{ brand }}</b-checkbox>
            </div>
        </div>

        <!-- Series -->
        <div class="has-margin-bottom-2">
            <p class="is-size-6 has-text-weight-bold has-margin-bottom-1">Series</p>

            <div v-for="s in series" :key="s">
                <b-checkbox :native-value="s" v-model="selectedSeries" @input="onInput">{{ s }}</b-checkbox>
            </div>
        </div>

        <!-- Category -->
        <div class="has-margin-bottom-2">
            <p class="is-size-6 has-text-weight-bold has-margin-bottom-1">Category</p>

            <div v-for="category in categories" :key="category">
                <b-checkbox
                    :native-value="category"
                    v-model="selectedCategories"
                    @input="onInput"
                >{{ category | padCategory }}</b-checkbox>
            </div>
        </div>

        <!-- Reset -->
        <b-button type="is-danger" outlined @click="onReset">Reset</b-button>
    </div>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { PadCategory } from '@/api';
import padStore from '../store/pad-store';
import { padCategory } from '@/modules/product-catalog/filters/pad-category';
import { FilterType } from '../store/filter-type';
import { Filter } from '../store/filter';
import store from '@/core/store';
import { MutationPayload } from 'vuex';

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
        return [...new Set(padStore.pads.map(p => p.series.brand.name))];
    }

    /**
     * Unique list of pad series names
     */
    get series() {
        return [...new Set(padStore.pads.map(p => p.series.name))];
    }

    get categories(): PadCategory[] {
        return ['heavy_cut', 'medium_cut', 'heavy_polish', 'medium_polish', 'soft_polish', 'finishing'];
    }

    selectedBrands: string[] = [];
    selectedSeries: string[] = [];
    selectedCategories: string[] = [];
    unSub!: () => void;

    created() {
        this.onReset();

        this.unSub = store.subscribe((mut: MutationPayload, state: any) => {
            if (mut.type == 'pad/SET_PADS') {
                this.onReset();
            }
        });
    }

    destroyed() {
        this?.unSub();
    }

    onInput() {
        padStore.SET_FILTER(
            new Filter(this.selectedBrands, this.selectedSeries, this.selectedCategories as PadCategory[])
        );
    }

    onReset() {
        this.selectedBrands = [...this.brands];
        this.selectedSeries = [...this.series];
        this.selectedCategories = [...this.categories];

        padStore.SET_FILTER(
            new Filter(this.selectedBrands, this.selectedSeries, this.selectedCategories as PadCategory[])
        );
    }
}
</script>