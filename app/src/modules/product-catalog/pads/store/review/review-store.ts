import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { Review } from '@/api/product-catalog/pad-series/data-transfer-objects/review';

@Module({ namespaced: true, name: 'review', dynamic: true, store })
class ReviewStore extends InitableModule {
    get stats() {
        return 1;
    }

    reviews: Review[] = [
        new Review('Test User', new Date(), 1, 4, 7, 'Not the best', 'Sucks'),
        new Review('admin', new Date(), 5, 10, 10, 'My new favorite', 'The bees knees'),
        new Review('okjoe', new Date(), 3, 5, 10, 'So-so', 'It ight'),
        new Review('okjoe', new Date(), 3, null, null, 'Not bad', 'It ight'),
    ];

    @Action({ rawError: true })
    loadReviews(padId: string) {
        console.log('loading!');
    }
}

export default getModule(ReviewStore);