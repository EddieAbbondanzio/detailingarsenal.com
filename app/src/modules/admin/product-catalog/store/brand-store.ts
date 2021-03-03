import { Module, VuexModule, Mutation, Action, getModule } from 'vuex-module-decorators';
import { InitableModule } from '@/core/store/initable-module';
import { api } from '@/api/api';
import store from '@/core/store/index';
import { SubscriptionPlan, Brand, BrandCreateRequest, BrandUpdateRequest } from '@/api';

@Module({ namespaced: true, name: 'brand', dynamic: true, store })
class BrandStore extends InitableModule {
    brands: Brand[] = [];

    @Mutation
    SET_BRANDS(brands: Brand[]) {
        this.brands = brands;
    }

    @Mutation
    CREATE_BRAND(brand: Brand) {
        this.brands.push(brand);
    }

    @Mutation
    UPDATE_BRAND(brand: Brand) {
        this.brands = [...this.brands.filter(b => b.id != brand.id), brand];
    }

    @Mutation
    DELETE_BRAND(brand: Brand) {
        const index = this.brands.findIndex(b => b.id == brand.id);

        if (index != -1) {
            this.brands.splice(index, 1);
        }
    }

    @Action({ rawError: true })
    async _init() {
        const [brands] = await Promise.all([api.productCatalog.brands.get()]);

        this.context.commit('SET_BRANDS', brands);
    }

    @Action({ rawError: true })
    async create(create: BrandCreateRequest) {
        const brand = await api.productCatalog.brands.create(create);
        this.context.commit('CREATE_BRAND', brand);
    }

    @Action({ rawError: true })
    async update(update: BrandUpdateRequest) {
        const brand = await api.productCatalog.brands.update(update);
        this.context.commit('UPDATE_BRAND', brand);
    }
    @Action({ rawError: true })
    async delete(brand: Brand) {
        await api.productCatalog.brands.delete(brand.id);
        this.context.commit('DELETE_BRAND', brand);
    }
}

export default getModule(BrandStore);
