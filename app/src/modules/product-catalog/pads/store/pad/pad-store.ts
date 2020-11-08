import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Pad, PadSeries, Brand, PadCategory, PadMaterial, PadSeriesSize, PolisherType, Rating, api } from '@/api';
import { PadTexture } from '@/api/product-catalog/pad-series/data-transfer-objects/pad-texture';

@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    pads: Pad[] = [];

    @Mutation
    SET_PADS(pads: Pad[]) {
        this.pads = pads;
    }

    @Action({ rawError: true })
    async _init() {
        const series = await api.productCatalog.padSeries.get();

        this.context.commit('SET_PADS', series.flatMap(s => s.pads));
    }

    @Action({ rawError: true })
    async getPadById(id: string): Promise<Pad | null> {
        throw new Error();
        // return this.pads[0]; //TODO: Fix
    }
}

export default getModule(PadStore);