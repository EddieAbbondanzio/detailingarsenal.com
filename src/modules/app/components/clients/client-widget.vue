<template>
    <div>
        <div class="is-flex is-flex-row is-align-items-center">
            <div
                class="client-icon has-background-light has-border-all-1 is-flex is-flex-column is-justify-content-center is-align-items-center"
            >
                <b-icon icon="account" type="is-dark" size="is-large" />
            </div>

            <div class="has-margin-x-3">
                <!-- Name / Edit link -->
                <div class="is-flex is-flex-row is-align-items-center has-margin-bottom-1">
                    <h1 class="title is-size-3 has-margin-bottom-0">{{ value.name }}</h1>
                    <router-link
                        class="has-margin-x-2"
                        :to="{name: 'editClient', params: { id: value.id }}"
                    >edit</router-link>
                </div>

                <!-- Contact Info -->
                <h2
                    class="subtitle is-size-5 has-margin-bottom-1 is-flex is-flex-row is-align-items-center"
                >
                    <b-icon icon="phone" />
                    <a
                        class="has-margin-x-1 has-text-dark"
                        :href="`tel:${value.phone}`"
                    >{{ value.phone }}</a>
                </h2>
                <h2 class="subtitle is-size-5 is-flex is-flex-row is-align-items-center">
                    <b-icon icon="email" />
                    <a
                        class="has-margin-x-1 has-text-dark"
                        :href="`mailto:${value.email}`"
                    >{{ value.email }}</a>
                </h2>
            </div>
        </div>
    </div>
</template>

<style lang="sass" scoped>
.client-icon
    border-radius: 8px
    border: 2px solid $grey
    height: 72px
    width: 72px
</style>

<script lang="ts">
import { Component, Vue, Prop } from 'vue-property-decorator';
import { Client } from '../../api';

@Component({
    name: 'client-widget'
})
export default class ClientWidget extends Vue {
    @Prop()
    public value!: Client;
}
</script>