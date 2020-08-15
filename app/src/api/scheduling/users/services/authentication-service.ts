import { Auth0Client } from '@auth0/auth0-spa-js';
import router from '@/core/router';
import { Route } from 'vue-router';

export class AuthenticationService {
    get isAuthenticated() {
        return this.isAuthed;
    }

    private isAuthed: boolean = true;
    private auth0!: Auth0Client;

    public async init() {
        this.auth0 = new Auth0Client({
            domain: process.env.VUE_APP_AUTH0_DOMAIN!,
            client_id: process.env.VUE_APP_AUTH0_CLIENT_ID!,
            audience: process.env.VUE_APP_AUTH0_AUDIENCE!
        });

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

    public async login(route: Route) {
        await this.auth0!.loginWithRedirect({
            redirect_uri: process.env.VUE_APP_AUTH0_CALLBACK_URI,
            appState: {
                route
            }
        });
    }

    public async logout() {
        this.auth0.logout();
        this.isAuthed = false;
    }

    public async getToken(): Promise<string> {
        return this.auth0.getTokenSilently();
    }
}
