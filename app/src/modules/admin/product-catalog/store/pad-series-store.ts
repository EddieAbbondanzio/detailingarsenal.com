import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { api } from '@/api/api';
import store from '@/core/store/index';
import { Pad, Brand, PadSeriesCreate, PadSeriesUpdate } from '@/api';
import { PadSeries } from '@/api';
import { PadSeriesService } from '@/api/product-catalog/pad-series/services/pad-series-service';

@Module({ namespaced: true, name: 'pad-series', dynamic: true, store })
class PadSeriesStore extends InitableModule {
    series: PadSeries[] = [];

    @Mutation
    SET_SERIES(series: PadSeries[]) {
        this.series = series;
    }

    @Mutation
    CREATE_SERIES(series: PadSeries) {
        this.series.push(series);
    }

    @Mutation
    UPDATE_SERIES(series: PadSeries) {
        this.series = [...this.series.filter(ps => ps.id != series.id), series];
    }

    @Mutation
    DELETE_SERIES(series: PadSeries) {
        const index = this.series.findIndex(ps => ps.id == series.id);
        if (index != -1) {
            this.series.splice(index, 1);
        }
    }

    @Action({ rawError: true })
    async _init() {
        const [series] = await Promise.all([api.productCatalog.padSeries.get()]);

        this.context.commit('SET_SERIES', series);
    }

    @Action({ rawError: true })
    async create(create: PadSeriesCreate) {
        const series = await api.productCatalog.padSeries.create(create);
        this.context.commit('CREATE_SERIES', series);
    }

    @Action({ rawError: true })
    async update(update: PadSeriesUpdate) {
        const series = await api.productCatalog.padSeries.update(update);
        this.context.commit('UPDATE_SERIES', series);
    }

    @Action({ rawError: true })
    async delete(padSeries: PadSeries) {
        await api.productCatalog.padSeries.delete(padSeries.id);
        this.context.commit('DELETE_SERIES', padSeries);
    }
}

export default getModule(PadSeriesStore);
