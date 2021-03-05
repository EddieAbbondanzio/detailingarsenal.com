import createAuth0Client, { Auth0Client } from '@auth0/auth0-spa-js';
import router from '@/core/router';
import { Route, RawLocation } from 'vue-router';

export class AuthenticationService {
    get isAuthenticated() {
        return this.isAuthed;
    }

    private isAuthed: boolean = true;
    private auth0!: Auth0Client;
    private initPromise: Promise<any> | null = null;

    async init() {
        this.initPromise = createAuth0Client({
            domain: process.env.VUE_APP_AUTH0_DOMAIN!,
            client_id: process.env.VUE_APP_AUTH0_CLIENT_ID!,
            audience: process.env.VUE_APP_AUTH0_AUDIENCE!
        });

        // This will try to log in the user after a refresh
        this.auth0 = await this.initPromise;
        this.initPromise = null;

        if (window.location.search.includes('code=') && window.location.search.includes('state=')) {
            const { appState } = await this.auth0.handleRedirectCallback();

            if (appState.route != null) {
                router.push(appState.route);
            }

            // Firefox bug
            window.location.hash = window.location.hash; // eslint-disable-line no-self-assign
            window.history.replaceState({}, document.title, window.location.pathname);
        }

        var user = await this.auth0.getUser();
        this.isAuthed = user != null;
    }

    /**
     * Redirect to the hosted login page.
     * @param route The route to return to.
     */
    async login(route: RawLocation | null) {
        await this.auth0!.loginWithRedirect({
            redirect_uri: process.env.VUE_APP_AUTH0_CALLBACK_URI,
            appState: {
                route
            }
        });
    }

    /**
     * Redirect to the hosted sign up page.
     * @param route The route to return to.
     */
    async signUp(route: RawLocation | null) {
        await this.auth0!.loginWithRedirect({
            redirect_uri: process.env.VUE_APP_AUTH0_CALLBACK_URI,
            appState: {
                route
            },
            screen_hint: 'signup'
        });
    }

    async logout() {
        this.auth0.logout();
        this.isAuthed = false;
    }

    async getToken(): Promise<string | null> {
        if (this.initPromise != null) {
            await this.initPromise;
        }

        if (!this.isAuthed) {
            return null;
        }

        try {
            var t = await this.auth0.getTokenSilently();
            return t;
        } catch (e) {
            return null;
        }
    }
}

export const authenticationService = new AuthenticationService();
