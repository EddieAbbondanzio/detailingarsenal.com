import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Pad, api } from '@/api';

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
        this.context.commit(
            'SET_PADS',
            series.flatMap(s => s.pads)
        );
    }

    @Action({ rawError: true })
    async getPadById(id: string): Promise<Pad | null> {
        if (this.pads.length == 0) {
            const series = await api.productCatalog.padSeries.get();
            this.context.commit(
                'SET_PADS',
                series.flatMap(s => s.pads)
            );
        }

        return this.pads.find(p => p.id == id)!;
    }
}

export default getModule(PadStore);
