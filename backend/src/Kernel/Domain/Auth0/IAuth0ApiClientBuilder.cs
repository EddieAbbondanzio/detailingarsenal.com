using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;

public interface IAuth0ApiClientBuilder {

    Task<AuthenticationApiClient> GetAuthenticationApiClient();
    Task<ManagementApiClient> GetManagementApiClient();
}