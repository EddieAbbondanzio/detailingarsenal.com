import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { PadSummary } from './pad-summary';
import { PadCategory, PadMaterial, PolisherType, Stars } from '@/api';

@Module({ namespaced: true, name: 'pad-summary-store', dynamic: true, store })
class PadSummaryStore extends InitableModule {
    summaries: PadSummary[] = [
        {
            id: '1',
            label: '5.5 Inch Lake Country CCS White Polishing',
            category: PadCategory.Polish,
            diameter: 5,
            thickness: 11 / 16,
            material: PadMaterial.Foam,
            stars: 3,
            reviewCount: 20,
            recommendedFor: [PolisherType.DualAction]
        }
    ];
}

export default getModule(PadSummaryStore);