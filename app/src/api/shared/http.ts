import axios from 'axios';
import { traverse } from '@/core/utils/traverse';
import { SpecificationError } from '@/api/shared/errors/specification-error';
import { ValidationError } from '@/api/shared/errors/validation-error';
import { AuthorizationError } from '@/api/shared/errors/authorization-error';
import router from '@/core/router';
import { authenticationService } from '../users';

/**
 * Singleton instance for making HTTP requests to backend.
 */
export const http = axios.create({
    baseURL: process.env.VUE_APP_API_DOMAIN,
    headers: { 'Content-Type': 'application/json' }
});

http.interceptors.request.use(async config => {
    /*
     * Cannot use user-store.ts here or else you'll introduce a circular dependency
     * which will throw a "rawModule is undefined" error and leave you banging your head
     * against the wall for hours.
     */

    // When authenticated, and sending a request to the backend add the token.
    if (config.baseURL == process.env.VUE_APP_API_DOMAIN && authenticationService.isAuthenticated) {
        const token = await authenticationService.getToken();
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

// trim out empty strings going out
http.interceptors.request.use(config => {
    config.transformRequest = (data, headers) => {
        var seen: any[] = [];

        // We remove any circular references by JSONifying it first.
        // https://stackoverflow.com/questions/9382167/serializing-object-that-contains-cyclic-object-value
        var jsonData = JSON.stringify(data, function(key, val) {
            if (val != null && typeof val == 'object') {
                if (seen.indexOf(val) >= 0) {
                    return;
                }
                seen.push(val);
            }
            return val;
        });

        traverse(jsonData, (obj, key, val) => {
            if (val == '') {
                delete obj[key];
            }
        });

        return jsonData;
    };

    return config;
});

// Look for incoming dates and parse them
http.interceptors.response.use(response => {
    traverse(response.data, (obj, key, val) => {
        if (
            typeof val == 'string' &&
            /^(-?(?:[1-9][0-9]*)?[0-9]{4})-(1[0-2]|0[1-9])-(3[01]|0[1-9]|[12][0-9])T(2[0-3]|[01][0-9]):([0-5][0-9]):([0-5][0-9])(.[0-9]+)?(Z)?$/.test(
                val
            )
        ) {
            obj[key] = new Date(val);
        }
    });

    return response;
});

// trim out incoming empty strings
http.interceptors.response.use(response => {
    traverse(response.data, (obj, key, val) => {
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
                    /*
                     * Assume (for now) that they tried to use a pro feature.
                     */
                    // router.push({ name: 'subscription' });
                    return Promise.reject(
                        new AuthorizationError("Uh Uh Uh. You didn't say the magic word")
                        // new AuthorizationError('Please upgrade your subscription to use this feature')
                    );
                }
            }
        }

        return Promise.reject(err);
    }
);
