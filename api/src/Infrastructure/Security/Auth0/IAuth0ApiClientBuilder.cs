using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.ManagementApi;

namespace DetailingArsenal.Infrastructure {
    public interface IAuth0ApiClientBuilder {

        Task<AuthenticationApiClient> GetAuthenticationApiClient();
        Task<ManagementApiClient> GetManagementApiClient();
    }
}