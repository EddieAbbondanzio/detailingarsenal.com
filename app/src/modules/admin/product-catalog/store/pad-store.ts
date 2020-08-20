import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { api } from '@/api/api';
import store from '@/core/store/index';
import { Pad, Brand } from '@/api';
import { PadSeries } from '@/api';
import { PadService } from '@/api/product-catalog/services/pad-service';

@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    series: PadSeries[] = [];

    @Mutation
    SET_SERIES(series: PadSeries[]) {
        this.series = series;
    }

    @Mutation
    CREATE_BRAND(brand: Pad) {
        // this.pads.push(brand);
    }

    @Mutation
    UPDATE_BRAND(brand: Pad) {
        // this.pads = [...this.pads.filter(b => b.id != brand.id), brand];
    }

    @Mutation
    DELETE_BRAND(pad: Pad) {
        // const index = this.pads.findIndex(b => b.id == pad.id);
        // if (index != -1) {
        //     this.pads.splice(index, 1);
        // }
    }

    @Action({ rawError: true })
    async _init() {
        // const [pads] = await Promise.all([api.productCatalog.pad.get()]);
        const series = [new PadSeries('1', 'CCS', new Brand('1', 'Lake Country'))];

        this.context.commit('SET_SERIES', series);
    }

    @Action({ rawError: true })
    async create() {}

    @Action({ rawError: true })
    async update() {}

    @Action({ rawError: true })
    async delete(pad: PadSeries) {}
}

export default getModule(PadStore);
