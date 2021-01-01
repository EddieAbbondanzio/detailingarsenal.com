<template>
    <page>
        <template v-slot:header>
            <page-header :title="service != null ? service.name : ``">
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{ name: 'settings' }" />
                        <breadcrumb name="Services" :to="{ name: 'services' }" />
                        <breadcrumb
                            :name="service != null ? service.name : ''"
                            :to="{ name: 'service', params: $route.params }"
                            :active="true"
                        />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <update-button :to="{ name: 'editService', params: { id: $route.params.id } }" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="service != null">
            <div class="has-margin-bottom-4">
                <h5 class="is-size-4 title has-margin-bottom-2">{{ service.name }}</h5>
                <span class="is-size-6 subtitle">{{ service.description }}</span>
            </div>

            <div v-if="service.configurations.length == 1">
                <div class="columns is-mobile">
                    <div class="column">
                        <div class="media">
                            <div class="media-left">
                                <b-icon icon="currency-usd" size="is-medium" />
                            </div>
                            <div class="media-content">
                                <p class="is-size-6 has-text-weight-bold">Price</p>
                                <p class="is-size-6">{{ service.configurations[0].price | currency }}</p>
                            </div>
                        </div>
                    </div>

                    <div class="column">
                        <div class="media">
                            <div class="media-left">
                                <b-icon icon="timelapse" size="is-medium" />
                            </div>
                            <div class="media-content">
                                <p class="is-size-6 has-text-weight-bold">Duration</p>
                                <p class="is-size-6">{{ service.configurations[0].duration | duration }}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="has-margin-bottom-3" v-else>
                <p class="is-size-5 has-text-weight-bold has-margin-bottom-1">Pricing</p>
                <hr class="has-margin-y-0" />

                <b-table :data="service.configurations">
                    <template slot-scope="props">
                        <b-table-column label="Vehicle category" field="vehicleCategoryId">{{
                            getVehicleCategoryName(props.row.vehicleCategoryId)
                        }}</b-table-column>

                        <b-table-column label="Price" field="price" numeric>{{
                            props.row.price | currency
                        }}</b-table-column>

                        <b-table-column label="Duration" field="duration" numeric>{{
                            props.row.duration | duration
                        }}</b-table-column>
                    </template>
                </b-table>
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import settingsStore from '../../store/settings-store';
import { Service } from '@/api';
import { displayLoading } from '@/core';

@Component({
    name: 'service',
})
export default class ServiceView extends Vue {
    service: Service = null!;

    @displayLoading
    async created() {
        await settingsStore.init();
        this.service = settingsStore.services.find((s) => s.id == this.$route.params.id)!;
    }

    getVehicleCategoryName(vcId: string) {
        const vc = settingsStore.vehicleCategories.find((vc) => vc.id == vcId);
        return vc == null ? 'Any' : vc.name;
    }
}
</script>