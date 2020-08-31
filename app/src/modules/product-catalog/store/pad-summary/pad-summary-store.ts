import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { PadSummary } from './pad-summary';
import { PadCategory, PadMaterial, PolisherType } from '@/api';

@Module({ namespaced: true, name: 'pad-summary-store', dynamic: true, store })
class PadSummaryStore extends InitableModule {
    summaries: PadSummary[] = [
        {
            id: '1',
            name: 'White Polishing',
            brandName: 'Lake Country',
            seriesName: 'CCS',
            category: PadCategory.Polish,
            diameter: '5.5 inch',
            material: PadMaterial.Foam,
            recommendedFor: [PolisherType.DualAction]
        }
    ];
}

export default getModule(PadSummaryStore);