import { extend, setInteractionMode } from 'vee-validate';
import * as rules from 'vee-validate/dist/rules';
import { configure } from 'vee-validate';
import Vue from 'vue';

configure({
    classes: {
        invalid: 'is-invalid'
    }
});

const messages = {
    alpha: '{_field_} may only contain alphabetic characters',
    alpha_num: '{_field_} may only contain alpha-numeric characters',
    alpha_dash: '{_field_} may contain alpha-numeric characters as well as dashes and underscores',
    alpha_spaces: '{_field_} may only contain alphabetic characters as well as spaces',
    between: '{_field_} must be between {min} and {max}',
    confirmed: '{_field_} confirmation does not match',
    digits: '{_field_} must be numeric and exactly contain {length} digits',
    dimensions: '{_field_} must be {width} pixels by {height} pixels',
    email: '{_field_} must be a valid email',
    excluded: '{_field_} is not a valid value',
    ext: '{_field_} is not a valid file',
    image: '{_field_} must be an image',
    integer: '{_field_} must be an integer',
    length: '{_field_} must be {length} long',
    max_value: '{_field_} must be {max} or less',
    max: '{_field_} may not be greater than {length} characters',
    mimes: '{_field_} must have a valid file type',
    min_value: '{_field_} must be {min} or more',
    min: '{_field_} must be at least {length} characters',
    numeric: '{_field_} may only contain numeric characters',
    oneOf: '{_field_} is not a valid value',
    regex: '{_field_} format is invalid',
    required_if: '{_field_} is required',
    required: '{_field_} is required',
    size: '{_field_} size must be less than {size}KB'
};

extend('required', { ...rules['required'], message: messages['required'] });
extend('min', { ...rules['min'], message: messages['min'] });
extend('min_value', { ...rules['min_value'], message: messages['min_value'] });
extend('max', { ...rules['max'], message: messages['max'] });
extend('email', { ...rules['email'], message: messages['email'] });
extend('confirmed', { ...rules['confirmed'], message: messages['confirmed'] });
extend('numeric', { ...rules['numeric'], message: messages['numeric'] });

extend('requiredIf', {
    params: ['target'],
    //@ts-ignore
    validate(value, { target }) {
        if (!target) {
            return true;
        } else {
            return value != null;
        }
    },
    message: '{_field_} is required'
});

extend('timeBefore', {
    params: ['target'],
    //@ts-ignore
    validate(value, { target }) {
        return value < target;
    },
    message: '{_field_} must be before {target}'
});

extend('timeAfter', {
    params: ['target'],
    //@ts-ignore
    validate(value, { target }) {
        return value > target;
    },
    message: '{_field_} must be after {target}'
});

extend('multipleOf', {
    params: ['factor'],
    //@ts-ignore
    validate(value, { factor }) {
        return value % factor == 0;
    },
    message: `{_field_} must be divisible by {factor}`
});

extend('decimal', {
    params: ['decimalPlaces'],
    //@ts-ignore
    validate(value, { decimalPlaces }) {
        if (isNaN(value)) {
            return false;
        }

        const splitInput = value.toString().split('.');

        if (splitInput.length < 2) {
            return true;
        }

        return splitInput[1].length <= decimalPlaces;
    },
    message: `{_field_} must be a decimal with {decimalPlaces} or less decimal places`
});
