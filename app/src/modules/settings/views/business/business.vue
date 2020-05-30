<template>
    <page>
        <template v-slot:header>
            <page-header
                title="Business"
                description="Name, address, and contact info"
                icon="domain"
                :backButtonTo="{name: 'settings'}"
            >
                <template v-slot:breadcrumb-trail>
                    <breadcrumb-trail>
                        <breadcrumb name="Settings" :to="{name: 'settings'}" />
                        <breadcrumb name="Business" :to="{name: 'business'}" :active="true" />
                    </breadcrumb-trail>
                </template>

                <template v-slot:action>
                    <edit-button :to="{name: 'editBusiness'}" />
                </template>
            </page-header>
        </template>

        <div class="box is-shadowless" v-if="business != null">
            <div class="has-margin-bottom-3">
                <h5 class="is-size-4 title has-margin-bottom-2">Name</h5>
                <span class="is-size-6 subtitle">{{ business.name }}</span>
            </div>

            <div class="has-margin-bottom-3">
                <h5 class="is-size-4 title has-margin-bottom-2">Address</h5>
                <span class="is-size-6 subtitle">{{ business.address}}</span>
            </div>

            <div class="has-margin-bottom-3">
                <h5 class="is-size-4 title has-margin-bottom-2">Phone</h5>
                <span class="is-size-6 subtitle">{{ business.phone }}</span>
            </div>
        </div>
    </page>
</template>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import settingsStore from '../../store/settings-store';
import { displayLoading } from '../../../../core/utils/display-loading';

@Component({
    name: 'business'
})
export default class BusinessView extends Vue {
    get business() {
        return settingsStore.business;
    }

    @displayLoading
    public async created() {
        await settingsStore.init();
    }
}
</script>