import Vue from 'vue';
import App from './app.vue';
import router from './core/router';
import store from './core/store';
import Buefy from 'buefy';
import '@/assets/styles/main.sass';
import '@/plugins/vee-validate';
import '@/plugins/vue2-touch-events';
import core from '@/core/plugin';
import { stripe } from '@/core/globals/stripe';

Vue.use(core);
Vue.use(Buefy);
Vue.config.productionTip = false;

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app');
