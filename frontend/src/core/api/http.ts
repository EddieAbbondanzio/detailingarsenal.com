import axios from 'axios';
import { api } from '@/modules/app/api/api';
import moment from 'moment';
import { traverse } from '@/core/utils/traverse';
import { SpecificationError } from '@/core/api/errors/specification-error';
import { ValidationError } from '@/core/api/errors/validation-error';
import { AuthorizationError } from '@/core/api/errors/authorization-error';

/**
 * Singleton instance for making HTTP requests to backend.
 */
export const http = axios.create({
    baseURL: process.env.VUE_APP_API_DOMAIN,
    headers: { 'Content-Type': 'application/json' }
});

http.interceptors.request.use(async config => {
    /*
     * Cannot user user-store.ts here or else you'll introduce a circular dependency
     * which will proceed to throw a "rawModule is undefined" error
     */

    // When authenticated, and sending a request to the backend add the token.
    if (config.baseURL == process.env.VUE_APP_API_DOMAIN && api.auth.isAuthenticated) {
        const token = await api.auth.getToken();
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

// trim out empty strings going out
http.interceptors.request.use(config => {
    config.transformRequest = (data, headers) => {
        traverse(data, (obj, key, val) => {
            if (val == '') {
                delete obj[key];
            }
        });

        return JSON.stringify(data);
    };

    return config;
});

// Look for incoming dates and parse them
http.interceptors.response.use(response => {
    traverse(response.data, (obj, key, val) => {
        if (
            typeof val == 'string' &&
            /\d{4}-[01]\d-[0-3]\dT[0-2]\d:[0-5]\d:[0-5]\d\.\d+([+-][0-2]\d:[0-5]\d|Z)/.test(val)
        ) {
            obj[key] = new Date(val);
        }
    });

    return response;
});

// trim out incoming empty strings
http.interceptors.response.use(response => {
    traverse(response, (obj, key, val) => {
        if (val == '') {
            delete obj[key];
        }
    });

    return response;
});

// Error generator.
http.interceptors.response.use(
    resp => resp,
    err => {
        if (err.response != null) {
            if (err.response.status == 400) {
                if (err.response.data.type == 'specification') {
                    return Promise.reject(
                        new SpecificationError(err.response.data.isSatisfied, err.response.data.message)
                    );
                } else if (err.response.data.type == 'validation') {
                    return Promise.reject(new ValidationError(err.response.data.valid, err.response.data.errors));
                }
            } else if (err.response.status == 401) {
                if (err.response.data.type == 'authorization') {
                    return Promise.reject(new AuthorizationError(err.response.data.message));
                }
            }
        }

        return Promise.reject(err);
    }
);
