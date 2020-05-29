import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import store from '@/core/store/index';

/**
 * Store for the schedule view.
 */
@Module({ namespaced: true, name: 'app', dynamic: true, store })
class AppStore extends InitableModule {
    loading = false;

    @Mutation
    LOADING() {
        this.loading = true;
    }

    @Mutation
    LOADED() {
        this.loading = false;
    }
}

export default getModule(AppStore);
