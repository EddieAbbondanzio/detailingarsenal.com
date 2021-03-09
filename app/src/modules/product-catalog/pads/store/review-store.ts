import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { PadReviews, Review, ReviewCreateRequest, reviewService } from '@/api/product-catalog';

@Module({ namespaced: true, name: 'review', dynamic: true, store })
class ReviewStore extends InitableModule {
    reviews: PadReviews = { rating: { reviewCount: 0, stars: null! }, stats: null!, values: [] };

    @Mutation
    SET_REVIEWS(reviews: PadReviews) {
        this.reviews = reviews;
    }

    @Mutation
    ADD_REVIEW(review: Review) {
        if (this.reviews.values == null) {
            this.reviews.values = [];
        }

        this.reviews.values.push(review);
    }

    @Action({ rawError: true })
    async loadReviews(padId: string) {
        const reviews = await reviewService.getForPad(padId);
        this.context.commit('SET_REVIEWS', reviews);
    }

    @Action({ rawError: true })
    async create(request: ReviewCreateRequest) {
        const r = await reviewService.create(request);

        this.context.commit('ADD_REVIEW', r);
    }
}

export default getModule(ReviewStore);
