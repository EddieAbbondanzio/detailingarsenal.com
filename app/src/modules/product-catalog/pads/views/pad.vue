<template>
    <page>
        <template v-slot:header>
            <page-header :title="value != null ? value.label : ``">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Pads" :to="{name: 'pads'}" />
                        <breadcrumb
                            :name="value != null ? value.label : ''"
                            :to="{name: 'pad', params: $route.params}"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless is-flex is-flex-column is-flex-grow-1" v-if="value != null">
            <div class="is-flex is-flex-row-desktop is-flex-column">
                <div
                    class="has-margin-right-3-desktop is-align-self-center is-align-self-start-desktop"
                >
                    <img
                        class="img is-square"
                        src="https://bulma.io/images/placeholders/480x480.png"
                    />
                </div>

                <div class="is-flex-grow-1">
                    <div class="has-margin-bottom-3">
                        <p class="is-size-4 is-size-3-desktop">{{value.label }}</p>
                        <stars :value="3" :count="20" />

                        <div class="columns">
                            <div class="column is-one-quarter">
                                <p class="is-size-6 has-text-weight-bold">Cut</p>
                                <pad-cut-bar :value="value.cut" />
                            </div>

                            <div class="column is-one-quarter">
                                <p class="is-size-6 has-text-weight-bold">Finish</p>
                                <pad-finish-bar :value="value.finish" />
                            </div>
                        </div>
                    </div>

                    <div class="columns has-margin-bottom-3">
                        <div class="column has-padding-bottom-3-touch">
                            <div class="has-margin-bottom-3">
                                <p class="is-size-5 title">Brand</p>
                                <p class="is-size-6 subtitle">{{ value.series.brand.name }}</p>
                            </div>

                            <div class="has-margin-bottom-3">
                                <p class="is-size-5 title">Series</p>
                                <p class="is-size-6 subtitle">{{ value.series.name }}</p>
                            </div>

                            <div class>
                                <p class="is-size-5 title">Name</p>
                                <p class="is-size-6 subtitle">{{ value.name }}</p>
                            </div>
                        </div>

                        <div class="column has-padding-top-0-touch">
                            <div class="has-margin-bottom-3">
                                <p class="is-size-5 title">Category</p>
                                <p class="is-size-6 subtitle">{{ value.category | uppercaseFirst }}</p>
                            </div>

                            <div class="has-margin-bottom-3">
                                <p class="is-size-5 title">Material</p>
                                <p class="is-size-6 subtitle">{{ value.material | uppercaseFirst }}</p>
                            </div>

                            <div class>
                                <p class="is-size-5 title">Texture</p>
                                <p class="is-size-6 subtitle">{{ value.texture | uppercaseFirst }}</p>
                            </div>
                        </div>
                    </div>

                    <div class="has-margin-bottom-3">
                        <p
                            class="is-size-5 title has-margin-bottom-1"
                        >Recommended For Polisher Type(s)</p>
                        <ul>
                            <li v-for="t in value.recommendedFor" :key="t">{{ t }}</li>
                        </ul>
                    </div>

                    <div class="has-margin-top-3">
                        <p class="is-size-5 title has-margin-bottom-1">Sizes</p>

                        <b-table :data="value.sizes">
                            <b-table-column
                                v-slot="props"
                                label="Diameter"
                                field="diameter"
                            >{{ props.row.diameter }}</b-table-column>
                            <b-table-column
                                v-slot="props"
                                label="Thickness"
                                field="thickness"
                            >{{ props.row.thickness }}</b-table-column>
                            <b-table-column
                                v-slot="props"
                                label="Part #"
                                field="partNumber"
                            >{{ props.row.partNumber }}</b-table-column>
                        </b-table>
                    </div>
                </div>
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Pad, PadSeries } from '@/api';
import padStore from '../store/pad/pad-store';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import PadCutBar from '@/modules/product-catalog/pads/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/pads/components/pad-finish-bar.vue';

@Component({
    components: {
        Stars,
        PadCutBar,
        PadFinishBar
    }
})
export default class PadView extends Vue {
    get id() {
        return this.$route.params.id;
    }

    get size() {
        if (this.$route.query.size == null) {
            return null;
        }

        return Number.parseFloat(this.$route.query.size as string);
    }

    value: Pad | null = null;

    async created() {
        this.value = await padStore.getPadById(this.id);
    }

    // size is a query string param.
    // Lets us mock the page to look like specifically that size
}
</script>