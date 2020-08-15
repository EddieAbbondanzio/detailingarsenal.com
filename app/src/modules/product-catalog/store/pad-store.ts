import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { Pad, api } from '@/api';
import store from '@/core/store/index';

/**
 * Store for the schedule view.
 */
@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    pads: Pad[] = [];

    @Mutation
    SET_PADS(pads: Pad[]) {
        this.pads = pads;
    }

    @Action({ rawError: true })
    async _init() {
        const [pads] = await Promise.all([api.productCatalog.pad.getPads()]);
        this.context.commit('SET_PADS', pads);
    }
}

export default getModule(PadStore);
