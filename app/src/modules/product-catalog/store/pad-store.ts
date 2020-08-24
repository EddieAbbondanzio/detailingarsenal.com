import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { Pad, api, PadCategory } from '@/api';
import store from '@/core/store/index';
import { Filter } from '@/modules/product-catalog/store/filter';
import { FilterType } from '@/modules/product-catalog/store/filter-type';

/**
 * Store for the schedule view.
 */
@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    get filtered() {
        if (this.filter.isEmpty) {
            return this.pads;
        }

        return this.pads.filter(
            p =>
                this.filter.brands.indexOf(p.series.brand.name) != -1 &&
                this.filter.series.indexOf(p.series.name) != -1 &&
                this.filter.category.indexOf(p.category) != -1
        );
    }

    pads: Pad[] = [];
    filter: Filter = new Filter();

    @Mutation
    SET_PADS(pads: Pad[]) {
        this.pads = pads;
    }

    @Mutation
    SET_FILTER(filter: Filter) {
        this.filter = filter;
    }

    @Action({ rawError: true })
    async _init() {
        const [pads] = await Promise.all([api.productCatalog.padSeries.get()]);
        this.context.commit('SET_PADS', pads);
    }
}

export default getModule(PadStore);
