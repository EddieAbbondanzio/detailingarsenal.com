<template>
    <div class="is-flex is-flex-column is-flex-grow-1">
        <navbar>
            <navbar-item :to="{ name: 'pads' }">Pads</navbar-item>
        </navbar>
        <div class="app-content">
            <section class="hero is-primary is-fullheight-with-navbar">
                <div class="hero-body">
                    <div class="container">
                        <p class="title">
                            Compare buffing pads by cut, finish, and more.
                        </p>
                        <p class="subtitle">
                            <b-button type="is-success" tag="router-link" :to="{ name: 'pads' }">View all</b-button>
                        </p>
                    </div>
                </div>
            </section>
        </div>

        <!-- <nav-footer>
            <nav-footer-item :to="{name: 'pads'}" icon="checkbox-blank-circle" text="Pads" />
        </nav-footer>-->
    </div>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';

@Component
export default class RootView extends Vue {}
</script>
