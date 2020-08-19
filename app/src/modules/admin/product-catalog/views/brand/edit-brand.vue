<template>
    <page>
        <template v-slot:header>
            <page-header title="Edit brand" :description="`Edit brand`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb
                            name="Product Catalog Panel"
                            :to="{name: 'productCatalogPanel'}"
                        />
                        <breadcrumb name="Brands" :to="{name: 'brands'}" />
                        <breadcrumb
                            name="Edit"
                            :to="{name: 'editBrand', params: $route.params}"
                            active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Save changes">
            <input-text-field
                label="Name"
                rules="required|max:32"
                :required="true"
                v-model="name"
                placeholder="Rupes"
            />
        </input-form>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop, Ref } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import ErrorMessage from '@/components/common/input/error-message.vue';
import ActionPage from '@/components/common/pages/action-page.vue';
import InputTextField from '@/core/components/input/input-text-field.vue';
import { toast, displayLoading, displayError } from '@/core';
import { ValidationError, SpecificationError } from '@/api';
import brandStore from '../../store/brand-store';

@Component
export default class EditBrand extends Vue {
    public name: string = '';

    @displayLoading
    async created() {
        const id = this.$route.params.id;
        await brandStore.init();

        const brand = brandStore.brands.find(b => b.id == id);

        if (brand == null) {
            throw new Error(`Brand with id ${id} does not exist.`);
        }

        this.name = brand.name;
    }

    @displayLoading
    public async onSubmit() {
        const update = { id: this.$route.params.id, name: this.name };

        try {
            await brandStore.update(update);

            toast(`Updated brand ${update.name}`);
            this.$router.push({ name: 'brands' });
        } catch (err) {
            if (err instanceof SpecificationError) {
                displayError(err);
            } else {
                throw err;
            }
        }
    }
}
</script>
