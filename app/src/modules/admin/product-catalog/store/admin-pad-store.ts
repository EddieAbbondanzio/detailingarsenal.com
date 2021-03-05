import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import padStore from '@/modules/product-catalog/pads/store/pad/pad-store';
import { padSeriesService, PadSeriesService } from '@/api/admin/services/pad-series-service';
import { PadSeries, PadSeriesCreateRequest, PadSeriesUpdateRequest } from '@/api/admin';
import { PagedArray } from '@/api/shared';

@Module({ namespaced: true, name: 'admin-pad', dynamic: true, store })
class AdminPadStore extends InitableModule {
    series: PagedArray<PadSeries> = { paging: { pageCount: 1, pageSize: 20, pageNumber: 0, total: 0 }, values: [] };

    @Mutation
    SET_SERIES(series: PagedArray<PadSeries>) {
        this.series = series;
    }

    @Mutation
    ADD_SERIES(series: PadSeries) {
        this.series.values.push(series);
        this.series.paging.total++;
    }

    @Mutation
    UPDATE_SERIES(series: PadSeries) {
        this.series.values = [...this.series.values.filter(ps => ps.id != series.id), series];
    }

    @Mutation
    DELETE_SERIES(series: PadSeries) {
        const index = this.series.values.findIndex(ps => ps.id == series.id);
        if (index != -1) {
            this.series.values.splice(index, 1);
            this.series.paging.total--;
        }
    }

    @Action({ rawError: true })
    async _init() {
        // const [series] = await Promise.all([padSeriesService.get()]);
        // this.context.commit('SET_SERIES', series);
    }

    @Action({ rawError: true })
    async goToPage(pageNumber: number) {
        // const series = await padSeriesService.get({
        //     paging: {
        //         pageNumber,
        //         pageSize: this.series.paging.pageSize
        //     }
        // });
    }

    @Action({ rawError: true })
    async create(create: PadSeriesCreateRequest) {
        const series = await padSeriesService.create(create);
        this.context.commit('ADD_SERIES', series);
    }

    @Action({ rawError: true })
    async update(update: PadSeriesUpdateRequest) {
        const series = await padSeriesService.update(update);
        this.context.commit('UPDATE_SERIES', series);
    }

    @Action({ rawError: true })
    async delete(padSeries: PadSeries) {
        await padSeriesService.delete(padSeries.id);
        this.context.commit('DELETE_SERIES', padSeries);
    }
}

export default getModule(AdminPadStore);
