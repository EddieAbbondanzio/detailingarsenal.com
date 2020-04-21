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
import InputSelect from './components/input/input-select.vue';
import InputCheckbox from './components/input/input-checkbox.vue';
import InputGroup from './components/input/input-group.vue';
import InputErrorMessage from './components/input/input-error-message.vue';
import InputDatepicker from '@/core/components/input/input-datepicker.vue';
import InputTimepicker from '@/core/components/input/input-timepicker.vue';
import BackButton from './components/buttons/back-button.vue';
import CreateButton from './components/buttons/create-button.vue';
import EditButton from './components/buttons/edit-button.vue';
import DeleteButton from './components/buttons/delete-button.vue';
import Page from './components/page/page.vue';
import PageHeader from './components/page/page-header.vue';
import BreadcrumbTrail from './components/page/breadcrumb-trail.vue';
import Breadcrumb from './components/page/breadcrumb.vue';
import List from './components/layout/list.vue';
import ListItem from './components/layout/list-item.vue';
import Phone from './components/elements/phone.vue';
import Email from './components/elements/email.vue';
import EditDeleteDropdown from './components/dropdowns/edit-delete-dropdown.vue';
import { ValidationProvider, ValidationObserver } from 'vee-validate';
import { disableAll } from '@/core/directives/disable-all';

export default {
    install(vue: typeof Vue) {
        Vue.component(BackButton.name, BackButton);
        Vue.component('create-button', CreateButton);
        Vue.component('edit-button', EditButton);
        Vue.component('delete-button', DeleteButton);
        Vue.component('page', Page);
        Vue.component('page-header', PageHeader);
        Vue.component('breadcrumb-trail', BreadcrumbTrail);
        Vue.component('breadcrumb', Breadcrumb);
        Vue.component('list', List);
        Vue.component('list-item', ListItem);
        Vue.component('validation-provider', ValidationProvider);
        Vue.component('validation-observer', ValidationObserver);
        Vue.component('input-form', InputForm);
        Vue.component('input-form-error-summary', InputFormErrorSummary);
        Vue.component('input-text-field', InputTextField);
        Vue.component('input-select', InputSelect);
        Vue.component('input-checkbox', InputCheckbox);
        Vue.component('input-group', InputGroup);
        Vue.component('input-error-message', InputErrorMessage);
        Vue.component(InputDatepicker.name, InputDatepicker);
        Vue.component(InputTimepicker.name, InputTimepicker);
        Vue.component('edit-delete-dropdown', EditDeleteDropdown);
        Vue.component(Phone.name, Phone);
        Vue.component(Email.name, Email);

        Vue.filter('currency', currency);
        Vue.filter('duration', duration);
        Vue.filter('nameOfDay', nameOfDay);
        Vue.filter('fullDate', fullDate);
        Vue.filter('date', date);
        Vue.filter('twelveHourFormat', twelveHourFormat);

        Vue.directive('disable-all', disableAll);
    }
};
