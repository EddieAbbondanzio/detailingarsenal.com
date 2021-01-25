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
import InputArray from '@/core/components/input/input-array.vue';
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

import { ValidationProvider, ValidationObserver } from 'vee-validate';
import { disableAll } from '@/core/directives/disable-all';
import { mouse } from '@/core/directives/mouse/mouse';
import { uppercase_first as uppercaseFirst } from '@/core/filters/uppercase_first';

export default {
    install(vue: typeof Vue) {
        Vue.component(BackButton.name, BackButton);
        Vue.component(CreateButton.name, CreateButton);
        Vue.component(UpdateButton.name, UpdateButton);
        Vue.component(DeleteButton.name, DeleteButton);
        Vue.component(Page.name, Page);
        Vue.component(PageHeader.name, PageHeader);
        Vue.component(PageSidebar.name, PageSidebar);
        Vue.component(BreadcrumbTrail.name, BreadcrumbTrail);
        Vue.component(Breadcrumb.name, Breadcrumb);
        Vue.component(List.name, List);
        Vue.component(ListItem.name, ListItem);
        Vue.component('validation-provider', ValidationProvider);
        Vue.component('validation-observer', ValidationObserver);
        Vue.component(InputForm.name, InputForm);
        Vue.component(InputFormErrorSummary.name, InputFormErrorSummary);
        Vue.component(InputTextField.name, InputTextField);
        Vue.component(InputAutocomplete.name, InputAutocomplete);
        Vue.component(InputSelect.name, InputSelect);
        Vue.component(InputCheckbox.name, InputCheckbox);
        Vue.component(InputGroup.name, InputGroup);
        Vue.component(InputErrorMessage.name, InputErrorMessage);
        Vue.component(InputArray.name, InputArray);
        Vue.component(InputImageUpload.name, InputImageUpload);
        Vue.component(InputDatepicker.name, InputDatepicker);
        Vue.component(InputTimepicker.name, InputTimepicker);
        Vue.component(InputSlider.name, InputSlider);
        Vue.component(InputTagger.name, InputTagger);
        Vue.component(UpdateDeleteDropdown.name, UpdateDeleteDropdown);
        Vue.component(Phone.name, Phone);
        Vue.component(Email.name, Email);
        Vue.component(Navbar.name, Navbar);
        Vue.component(NavbarItem.name, NavbarItem);
        Vue.component(NavFooter.name, NavFooter);
        Vue.component(NavFooterItem.name, NavFooterItem);

        Vue.filter('currency', currency);
        Vue.filter('duration', duration);
        Vue.filter('nameOfDay', nameOfDay);
        Vue.filter('fullDate', fullDate);
        Vue.filter('date', date);
        Vue.filter('twelveHourFormat', twelveHourFormat);
        Vue.filter('uppercaseFirst', uppercaseFirst);

        Vue.directive('disable-all', disableAll);
        Vue.directive('mouse', mouse);
    }
};
