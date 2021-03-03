import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';
import { api, Review, ReviewCreateRequest } from '@/api';

@Module({ namespaced: true, name: 'review', dynamic: true, store })
class ReviewStore extends InitableModule {
    reviews: Review[] = [];

    @Mutation
    SET_REVIEWS(reviews: Review[]) {
        this.reviews = reviews;
    }

    @Mutation
    ADD_REVIEW(review: Review) {
        this.reviews.push(review);
    }

    @Action({ rawError: true })
    async loadReviews(padId: string) {
        const reviews = await api.productCatalog.reviews.getForPad(padId);
        this.context.commit('SET_REVIEWS', reviews);
    }

    @Action({ rawError: true })
    async create(request: ReviewCreateRequest) {
        const r = await api.productCatalog.reviews.create(request);

        this.context.commit('ADD_REVIEW', r);
    }
}

export default getModule(ReviewStore);