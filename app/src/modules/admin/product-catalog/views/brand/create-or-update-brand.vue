<template>
    <page>
        <template v-slot:header>
            <page-header :title="`${verb} brand`" :description="`${verb} brand`">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Admin panel" :to="{ name: 'adminPanel' }" />
                        <breadcrumb name="Product catalog panel" :to="{ name: 'productCatalogPanel' }" />
                        <breadcrumb name="Brands" :to="{ name: 'brands' }" />
                        <breadcrumb
                            v-if="mode == 'update'"
                            :name="name"
                            :to="{ name: 'brand', params: $route.params }"
                        />
                        <breadcrumb :name="verb" :to="$route" :active="true" />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <input-form @submit="onSubmit" :submitText="verb">
            <input-text-field
                label="Name"
                rules="required|max:32"
                :required="true"
                v-model="name"
                placeholder="Rupes"
                v-focus
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
import InputViewMixin from '@/core/mixins/input-view-mixin';

@Component
export default class CreateOrUpdateBrand extends InputViewMixin {
    name: string = '';

    @displayLoading
    async created() {
        if (this.mode == 'update') {
            await brandStore.init();

            const brand = brandStore.brands.find(b => b.id == this.id);

            if (brand == null) {
                this.$router.go(-1);
                return;
            }

            this.name = brand.name;
        }
    }

    @displayLoading
    async onSubmit() {
        try {
            if (this.mode == 'create') {
                await brandStore.create({
                    name: this.name
                });
            } else {
                await brandStore.update({
                    id: this.id,
                    name: this.name
                });
            }

            toast(`${this.verb}d brand ${this.name}`);
            this.$router.push({ name: 'brands' });
        } catch (err) {
            displayError(err);
        }
    }
}
</script>
