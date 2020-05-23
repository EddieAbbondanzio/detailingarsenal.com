import Auth0Client from '@auth0/auth0-spa-js/dist/typings/Auth0Client';
import createAuth0Client from '@auth0/auth0-spa-js';

export class AuthService {
    get isAuthenticated() {
        return this.isAuthed;
    }

    private isAuthed: boolean = false;
    private auth0!: Auth0Client;

    public async init() {
        this.auth0 = await createAuth0Client({
            domain: process.env.VUE_APP_AUTH0_DOMAIN!,
            client_id: process.env.VUE_APP_AUTH0_CLIENT_ID!,
            audience: process.env.VUE_APP_AUTH0_AUDIENCE!
        });

        if (window.location.search.includes('code=') && window.location.search.includes('state=')) {
            await this.auth0.handleRedirectCallback();
            // Firefox bug
            window.location.hash = window.location.hash; // eslint-disable-line no-self-assign
            window.history.replaceState({}, document.title, window.location.pathname);
        }

        var user = await this.auth0.getUser();
        this.isAuthed = user != null;
    }

    public async login() {
        await this.auth0!.loginWithRedirect({ redirect_uri: process.env.VUE_APP_AUTH0_CALLBACK_URI });
    }

    public async logout() {
        this.auth0.logout();
        this.isAuthed = false;
    }

    public async getToken(): Promise<string> {
        return this.auth0.getTokenSilently();
    }
}
