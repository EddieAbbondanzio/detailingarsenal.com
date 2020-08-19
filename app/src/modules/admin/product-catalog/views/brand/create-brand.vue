<template>
    <page>
        <template v-slot:header>
            <page-header title="Create brand" :description="`Create brand`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin Panel" :to="{name: 'adminPanel'}" />
                        <breadcrumb
                            name="Product Catalog Panel"
                            :to="{name: 'productCatalogPanel'}"
                        />
                        <breadcrumb name="Brands" :to="{name: 'brands'}" />
                        <breadcrumb name="Create" :to="{name: 'createBrand'}" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" submitText="Create">
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
export default class CreateBrand extends Vue {
    public name: string = '';

    @displayLoading
    public async onSubmit() {
        const create = { name: this.name };

        try {
            await brandStore.create({
                name: this.name
            });

            toast(`Created new brand ${create.name}`);
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
