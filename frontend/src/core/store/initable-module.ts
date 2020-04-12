import { VuexModule, Action, Mutation } from 'vuex-module-decorators';

/**
 * Vuex module that has an init life cycle event to pre-load data
 * from the backend. Add an action called _init to the derived class to use.
 */
export abstract class InitableModule extends VuexModule {
    isInitialized: boolean = false;
    private initPromise: Promise<void> | null = null;

    @Mutation
    private IS_INITIALIZED() {
        this.isInitialized = true;
    }

    @Mutation
    private SET_INIT_PROMISE(initPromise: Promise<void>) {
        this.initPromise = initPromise;
    }

    @Action({ rawError: true })
    public async init() {
        // Don't init if we've already initialized.
        if (this.isInitialized) {
            return;
        }

        // Check to see if an init is in progress.
        if (this.initPromise != null) {
            return this.initPromise;
        }

        // Trigger the init
        const p = this.context.dispatch('_init');

        this.context.commit('SET_INIT_PROMISE', p);
        await p;
        this.context.commit('IS_INITIALIZED');
    }
}
