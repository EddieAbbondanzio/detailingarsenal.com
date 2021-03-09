<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Buffing pad catalog"
                :description="`Compare buffing pads of multiple brands based on cut, finish, size, etc...`"
                :backButton="false"
            >
            </page-header>
        </template>

        <template v-slot:sidebar>
            <page-sidebar>
                <pad-filter-control />
            </page-sidebar>
        </template>

        <div
            class="box is-shadowless has-padding-all-3 has-margin-bottom-3 is-flex is-flex-row is-align-items-center is-justify-content-space-between"
        >
            <p class="title is-6 is-5-tablet has-margin-bottom-0">{{ paging.total }} Pads</p>

            <b-button class="is-hidden-tablet " type="is-primary" outlined @click="showSidebar">Filter</b-button>
        </div>

        <b-table
            class="pads-table"
            :data="pads"
            :loading="loading"
            paginated
            backend-pagination
            :total="paging.total"
            :per-page="paging.pageSize"
            @page-change="onPageChange"
        >
            <b-table-column v-slot="props" centered>
                <router-link
                    :to="{
                        name: 'pad',
                        params: { padId: props.row.id }
                    }"
                >
                    <img
                        :src="
                            props.row.thumbnailUrl != null
                                ? props.row.thumbnailUrl
                                : 'https://via.placeholder.com/60x60?text=NA'
                        "
                        class="pad-thumb"
                    />
                </router-link>
            </b-table-column>
            <b-table-column v-slot="props" label="Name" field="label">
                <router-link
                    class="label-link has-text-weight-bold"
                    :to="{
                        name: 'pad',
                        params: {
                            padId: props.row.id
                        }
                    }"
                    >{{ props.row.name }}
                    <b-tag type="is-info" v-if="props.row.isThin" size="is-small">Thin</b-tag>
                </router-link>
            </b-table-column>
            <b-table-column v-slot="props" label="Category" field="category">{{
                props.row.category | uppercaseFirst | commaSeperate
            }}</b-table-column>
            <b-table-column v-slot="props" label="Material" field="material">
                <span v-if="props.row.material">{{ props.row.material | uppercaseFirst }}</span>
                <span v-else class="has-text-grey">N/A</span>
            </b-table-column>
            <b-table-column label="Cut" field="cut" width="120px" v-slot="props">
                <pad-cut-bar :value="props.row.cut" />
            </b-table-column>
            <b-table-column v-slot="props" label="Finish" field="finish" width="120px">
                <pad-finish-bar :value="props.row.finish" />
            </b-table-column>
            <b-table-column v-slot="props" label="Rating" field="rating">
                <stars
                    v-if="props.row.rating != null"
                    :value="props.row.rating.stars"
                    :count="props.row.rating.reviewCount"
                    size="is-small"
                    :readOnly="true"
                />
                <span class="has-text-grey" v-else>N/A</span>
            </b-table-column>
            <b-table-column v-slot="props" label="Polisher Type(s)" field="polisherTypes">
                <div class="tags">
                    <polisher-type-tag
                        v-for="rec in props.row.polisherTypes"
                        :key="rec"
                        :value="rec"
                    ></polisher-type-tag>
                </div>
            </b-table-column>

            <template slot="empty">
                <div class="is-flex is-justify-content-center">There's nothing here!</div>
            </template>
        </b-table>
    </page>
</template>

<style lang="sass">
.pad-thumb
    max-height: 40px!important

.label-link
    color: $dark!important

    &:hover
        color: $primary!important

.pads-table
    td
        margin: auto!important
        vertical-align: middle
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { displayLoading } from '@/core';
import PadFilterControl from '@/modules/product-catalog/pads/components/pad-filter-control.vue';
import store from '@/core/store';
import { MutationPayload } from 'vuex';
import PageSidebar from '@/core/components/page/page-sidebar.vue';
import PadCutBar from '@/modules/product-catalog/pads/components/pad-cut-bar.vue';
import PadFinishBar from '@/modules/product-catalog/pads/components/pad-finish-bar.vue';
import Stars from '@/modules/product-catalog/core/components/stars.vue';
import padStore from '../store/pad-store';
import PolisherTypeTag from '@/modules/shared/components/polisher-type-tag.vue';
import appStore from '@/core/store/app-store';
import { uppercaseFirst } from '@/core/filters/uppercase-first';
import { Rating } from '@/api/product-catalog';
import { PadMaterial, PolisherType } from '@/api/shared';

@Component({
    components: {
        PadFilterControl,
        PadCutBar,
        PadFinishBar,
        Stars,
        PolisherTypeTag
    }
})
export default class Pads extends Vue {
    get paging() {
        return padStore.pads.paging;
    }

    get pads() {
        return padStore.pads.values;
    }

    loading = false; // used for b-table

    @displayLoading
    async created() {
        this.loading = true;
        await padStore.init();
        this.loading = false;

        // Pull in remainder pads
        if (padStore.pads.values.length == 1) {
            await padStore.getAll();
        }
    }

    @displayLoading
    async onPageChange(pageNumber: number) {
        this.loading = true;
        await padStore.goToPage(pageNumber);
        this.loading = false;
    }

    showSidebar() {
        appStore.SHOW_SIDEBAR();
    }
}
</script>
