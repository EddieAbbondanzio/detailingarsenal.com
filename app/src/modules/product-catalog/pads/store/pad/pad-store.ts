import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import {
    Pad,
    api,
    PadSeriesFilterLegend,
    PadSeries,
    PagedArray,
    Paging,
    PadSeriesGetAllRequest,
    PadSeriesFilter
} from '@/api';
import _ from 'lodash';

@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    get pads() {
        return this.series?.values.flatMap(s => s.pads) ?? [];
    }

    legend: PadSeriesFilterLegend = { brands: [], series: [] };
    filter: PadSeriesFilter = {};
    series: PagedArray<PadSeries> = { paging: { pageCount: 1, pageSize: 20, pageNumber: 0, total: 0 }, values: [] };

    @Mutation
    SET_LEGEND(legend: PadSeriesFilterLegend) {
        this.legend = legend;
    }

    @Mutation
    SET_SERIES(series: PagedArray<PadSeries>) {
        this.series = series;
    }

    @Mutation
    SET_FILTER(filter: PadSeriesFilter) {
        this.filter = filter;
    }

    @Action({ rawError: true })
    async _init() {
        const f = await api.productCatalog.padSeriesFilter.get();
        this.context.commit('SET_LEGEND', f);

        const series = await api.productCatalog.padSeries.get();
        this.context.commit('SET_SERIES', series);
    }

    @Action({ rawError: true })
    async reloadFilter() {
        const f = await api.productCatalog.padSeriesFilter.get();
        this.context.commit('SET_LEGEND', f);
        console.log('reload!');
    }

    @Action({ rawError: true })
    async getAll(filter?: PadSeriesFilter) {
        const series = await api.productCatalog.padSeries.get({
            ...filter,
            paging: this.series.paging
        });

        this.context.commit('SET_SERIES', series);
        this.context.commit('SET_FILTER', filter);
    }

    @Action({ rawError: true })
    async goToPage(pageNumber: number) {
        const series = await api.productCatalog.padSeries.get({
            ...this.filter,
            paging: {
                pageNumber,
                pageSize: this.series.paging.pageSize
            }
        });
    }

    @Action({ rawError: true })
    async getAllBySeries(id: string) {
        var s = await api.productCatalog.padSeries.getById(id);
        this.context.commit('SET_SERIES', s);
    }
}

export default getModule(PadStore);
