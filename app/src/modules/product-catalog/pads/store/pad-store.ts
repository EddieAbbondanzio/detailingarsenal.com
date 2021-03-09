import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import _ from 'lodash';
import {
    PadFilter,
    PadFilterLegend,
    padFilterService,
    padService,
    PadSize,
    padSizeService
} from '@/api/product-catalog';
import { PagedArray, Paging } from '@/api/shared';
import { Pad } from '@/api/product-catalog/data-transfer-objects/pad';

@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    legend: PadFilterLegend = { brands: [], series: [] };
    filter: PadFilter = {};
    pads: Pad[] = [];
    sizes: PadSize[] = [];
    paging: Paging = { pageCount: 1, pageNumber: 0, pageSize: 20, total: 0 };

    @Mutation
    SET_LEGEND(legend: PadFilterLegend) {
        this.legend = legend;
    }

    @Mutation
    SET_FILTER(filter: PadFilter) {
        this.filter = filter;
    }

    @Mutation
    SET_PADS(pads: Pad[]) {
        this.pads = pads;
    }

    @Mutation
    SET_SIZES(sizes: PadSize[]) {
        this.sizes = sizes;
    }

    @Mutation
    CLEAR_SIZES() {
        this.sizes.length = 0;
    }

    @Mutation
    SET_PAGING(paging: Paging) {
        this.paging = paging;
    }

    @Action({ rawError: true })
    async _init() {
        const [filter, pads] = await Promise.all([padFilterService.get(), padService.getAll()]);

        this.context.commit('SET_LEGEND', filter);
        this.context.commit('SET_PADS', pads.values);
        this.context.commit('SET_PAGING', pads.paging);
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
            paging: this.paging
        });

        this.context.commit('SET_PADS', pads.values);
        this.context.commit('SET_FILTER', filter);
        this.context.commit('SET_PAGING', pads.values);
    }

    @Action({ rawError: true })
    async get(id: string) {
        const pad = await padService.get(id);
        this.context.commit('SET_PADS', [pad]);
        this.context.commit('SET_PAGING', { pageCount: 1, pageNumber: 0, pageSize: 20, total: 0 });
    }

    @Action({ rawError: true })
    async goToPage(pageNumber: number) {
        const pads = await padService.getAll({
            ...this.filter,
            paging: {
                pageNumber,
                pageSize: this.paging.pageSize
            }
        });

        this.context.commit('SET_PADS', pads.values);
        this.context.commit('SET_PAGING', pads.paging);
    }

    @Action({ rawError: true })
    async getSizes(padId: string) {
        // Remove existing sizes in case we've already visited another pad view
        this.context.commit('CLEAR_SIZES');

        const sizes = await padSizeService.getForPad(padId);
        this.context.commit('SET_SIZES', sizes);
    }
}
export default getModule(PadStore);
