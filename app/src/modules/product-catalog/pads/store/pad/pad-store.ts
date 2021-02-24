import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Pad, api, PadSeriesFilter, PadSeries, PagedArray, Paging } from '@/api';
import _ from 'lodash';

@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    get pads() {
        return this.series?.values.flatMap(s => s.pads) ?? [];
    }

    filter: PadSeriesFilter = { brands: [], series: [] };
    paging: Paging = { pageNumber: 0, pageSize: 20 };

    series: PagedArray<PadSeries> = { paging: null!, values: [] };

    @Mutation
    SET_FILTER(filter: PadSeriesFilter) {
        this.filter = filter;
    }

    @Mutation
    SET_SERIES(series: PagedArray<PadSeries>) {
        this.series = series;
    }

    @Action({ rawError: true })
    async _init() {
        const f = await api.productCatalog.padSeriesFilter.get();
        this.context.commit('SET_FILTER', f);

        const series = await api.productCatalog.padSeries.get();
        this.context.commit('SET_SERIES', series);
    }

    @Action({ rawError: true })
    async getAll(filter?: PadSeriesFilter) {
        const series = await api.productCatalog.padSeries.get({
            brands: this.filter.brands?.map(b => b.id),
            series: this.filter.series?.map(s => s.id),
            paging: this.paging
        });

        this.context.commit('SET_SERIES', series);
    }

    @Action({ rawError: true })
    async getAllBySeries(id: string) {
        var s = await api.productCatalog.padSeries.getById(id);
        this.context.commit('SET_SERIES', s);
    }
}

export default getModule(PadStore);
