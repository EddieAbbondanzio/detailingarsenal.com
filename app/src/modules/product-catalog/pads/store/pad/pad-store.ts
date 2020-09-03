import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Pad, PadSeries, Brand, PadCategory, PadMaterial, PadSize, PolisherType } from '@/api';
import { PadTexture } from '@/api/product-catalog/pad-series/data-transfer-objects/pad-texture';

@Module({ namespaced: true, name: 'pad', dynamic: true, store })
class PadStore extends InitableModule {
    pads: Pad[] = [];

    @Action({ rawError: true })
    async getPadById(id: string): Promise<Pad | null> {
        return new Pad(
            '1',
            new PadSeries('2', 'CCS', new Brand('3', 'Lake Country')),
            'White Polishing',
            PadCategory.Polish,
            3,
            7,
            PadMaterial.Foam,
            PadTexture.Dimpled,
            [new PadSize(5.5, 1.25)],
            [PolisherType.DualAction],
        )
    }
}

export default getModule(PadStore);