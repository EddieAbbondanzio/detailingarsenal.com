import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Pad, api, PadSeriesFilter, PadSeries, PagedArray } from '@/api';
import _ from 'lodash';

@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    get pads() {
        return this.series?.values.flatMap(s => s.pads) ?? [];
    }

    filter: PadSeriesFilter = null!;
    defaultFilter: PadSeriesFilter = null!;

    series: PagedArray<PadSeries> = null!;

    @Mutation
    SET_FILTER(filter: PadSeriesFilter) {
        this.filter = filter;
    }

    @Mutation
    SET_DEFAULT_FILTER(filter: PadSeriesFilter) {
        this.defaultFilter = filter;
    }

    @Mutation
    SET_SERIES(series: PagedArray<PadSeries>) {
        this.series = series;
    }

    @Action({ rawError: true })
    async _init() {
        const f = await api.productCatalog.padSeriesFilter.get();
        this.context.commit('SET_FILTER', f);
        this.context.commit('SET_DEFAULT_FILTER', _.cloneDeep(f));
        this.context.dispatch('getAll', f);
    }

    @Action({ rawError: true })
    async getAll(filter?: PadSeriesFilter) {
        const series = await api.productCatalog.padSeries.get(filter);
        this.context.commit('SET_SERIES', series);
    }

    @Action({ rawError: true })
    async getAllBySeries(id: string) {
        var s = await api.productCatalog.padSeries.getById(id);
        this.context.commit('SET_SERIES', s);
    }
}

export default getModule(PadStore);
