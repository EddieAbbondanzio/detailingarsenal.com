import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import _ from 'lodash';
import { PadFilter, PadFilterLegend, padFilterService, padService } from '@/api/product-catalog';
import { PagedArray } from '@/api/shared';
import { Pad } from '@/api/product-catalog/data-transfer-objects/pad';

@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    legend: PadFilterLegend = { brands: [], series: [] };
    filter: PadFilter = {};
    pads: PagedArray<Pad> = { paging: { pageCount: 1, pageSize: 20, pageNumber: 0, total: 0 }, values: [] };

    @Mutation
    SET_LEGEND(legend: PadFilterLegend) {
        this.legend = legend;
    }

    @Mutation
    SET_PADS(pads: PagedArray<Pad>) {
        this.pads = pads;
    }

    @Mutation
    SET_FILTER(filter: PadFilter) {
        this.filter = filter;
    }

    @Action({ rawError: true })
    async _init() {
        const [filter, pads] = await Promise.all([padFilterService.get(), padService.getAll()]);

        this.context.commit('SET_LEGEND', filter);
        this.context.commit('SET_PADS', pads);
    }

    @Action({ rawError: true })
    async reloadFilter() {
        const f = await padFilterService.get();
        this.context.commit('SET_LEGEND', f);
    }

    @Action({ rawError: true })
    async getAll(filter?: PadFilter) {
        const pads = await padService.getAll({
            ...filter,
            paging: this.pads.paging
        });

        this.context.commit('SET_PADS', pads);
        this.context.commit('SET_FILTER', filter);
    }

    @Action({ rawError: true })
    async get(id: string) {
        const pads = await padService.get(id);

        this.context.commit('SET_PADS', [pads]);
    }

    @Action({ rawError: true })
    async goToPage(pageNumber: number) {
        const series = await padService.getAll({
            ...this.filter,
            paging: {
                pageNumber,
                pageSize: this.pads.paging.pageSize
            }
        });
    }
}

export default getModule(PadStore);
