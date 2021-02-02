import Vue from 'vue';

import { duration } from '@/core/filters/duration';
import { nameOfDay } from '@/core/filters/name-of-day';
import { twelveHourFormat } from '@/core/filters/twelve-hour-format';
import { currency } from '@/core/filters/currency';
import { fullDate } from '@/core/filters/full-date';
import { date } from '@/core/filters/date';

import InputForm from './components/input/input-form.vue';
import InputFormErrorSummary from './components/input/input-form-error-summary.vue';
import InputTextField from './components/input/input-text-field.vue';
import InputAutocomplete from './components/input/input-autocomplete.vue';
import InputSelect from './components/input/input-select.vue';
import InputCheckbox from './components/input/input-checkbox.vue';
import InputGroup from './components/input/input-group.vue';
import InputErrorMessage from './components/input/input-error-message.vue';
import InputDatepicker from '@/core/components/input/input-datepicker.vue';
import InputTimepicker from '@/core/components/input/input-timepicker.vue';
import InputSlider from '@/core/components/input/input-slider.vue';
import InputImageUpload from '@/core/components/input/input-image-upload.vue';
import InputTagger from '@/core/components/input/input-tagger.vue';
import BackButton from './components/buttons/back-button.vue';
import CreateButton from './components/buttons/create-button.vue';
import UpdateButton from './components/buttons/update-button.vue';
import DeleteButton from './components/buttons/delete-button.vue';
import Page from './components/page/page.vue';
import PageHeader from './components/page/page-header.vue';
import PageSidebar from './components/page/page-sidebar.vue';
import BreadcrumbTrail from './components/page/breadcrumb-trail.vue';
import Breadcrumb from './components/page/breadcrumb.vue';
import List from './components/layout/list.vue';
import ListItem from './components/layout/list-item.vue';
import Phone from './components/elements/phone.vue';
import Email from './components/elements/email.vue';
import UpdateDeleteDropdown from './components/dropdowns/update-delete-dropdown.vue';
import Navbar from '@/core/components/navigation/navbar.vue';
import NavbarItem from '@/core/components/navigation/navbar-item.vue';
import NavFooter from '@/core/components/navigation/nav-footer.vue';
import NavFooterItem from '@/core/components/navigation/nav-footer-item.vue';
import ImageThumbnail from '@/core/components/elements/image-thumbnail.vue';

import { ValidationProvider, ValidationObserver } from 'vee-validate';
import { disableAll } from '@/core/directives/disable-all';
import { mouse } from '@/core/directives/mouse/mouse';
import { uppercaseFirst } from '@/core/filters/uppercase-first';
import { focusDirective } from './directives/focus';

export default {
    install(vue: typeof Vue) {
        /**
         *  DON'T USE Class.name. It'll work in dev, but break in production due to minifying code
         *  having different class names.
         */

        Vue.component('back-button', BackButton);
        Vue.component('create-button', CreateButton);
        Vue.component('update-button', UpdateButton);
        Vue.component('delete-button', DeleteButton);
        Vue.component('page', Page);
        Vue.component('page-header', PageHeader);
        Vue.component('page-sidebar', PageSidebar);
        Vue.component('breadcrumb-trail', BreadcrumbTrail);
        Vue.component('breadcrumb', Breadcrumb);
        Vue.component('list', List);
        Vue.component('list-item', ListItem);
        Vue.component('validation-provider', ValidationProvider);
        Vue.component('validation-observer', ValidationObserver);
        Vue.component('input-form', InputForm);
        Vue.component('input-form-error-summary', InputFormErrorSummary);
        Vue.component('input-text-field', InputTextField);
        Vue.component('input-autocomplete', InputAutocomplete);
        Vue.component('input-select', InputSelect);
        Vue.component('input-checkbox', InputCheckbox);
        Vue.component('input-group', InputGroup);
        Vue.component('input-error-message', InputErrorMessage);
        Vue.component('input-image-upload', InputImageUpload);
        Vue.component('input-datepicker', InputDatepicker);
        Vue.component('input-timepicker', InputTimepicker);
        Vue.component('input-slider', InputSlider);
        Vue.component('input-trigger', InputTagger);
        Vue.component('update-delete-dropdown', UpdateDeleteDropdown);
        Vue.component('phone', Phone);
        Vue.component('email', Email);
        Vue.component('navbar', Navbar);
        Vue.component('navbar-item', NavbarItem);
        Vue.component('nav-footer', NavFooter);
        Vue.component('nav-footer-item', NavFooterItem);
        Vue.component('image-thumbnail', ImageThumbnail);

        Vue.filter('currency', currency);
        Vue.filter('duration', duration);
        Vue.filter('nameOfDay', nameOfDay);
        Vue.filter('fullDate', fullDate);
        Vue.filter('date', date);
        Vue.filter('twelveHourFormat', twelveHourFormat);
        Vue.filter('uppercaseFirst', uppercaseFirst);

        Vue.directive('disable-all', disableAll);
        Vue.directive('mouse', mouse);
        Vue.directive('focus', focusDirective);
    }
};
