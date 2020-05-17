using System;
using System.Threading.Tasks;
using Auth0.ManagementApi;

public interface IUserService : IService {
    Task<User?> GetUserByAuth0Id(string auth0Id);
    Task<User> GetOrCreateUserByAuth0Id(string auth0Id);
    Task<User?> GetUserById(string id);
}

public class UserService : IUserService {
    private IAuth0ApiClientBuilder tokenGenerator;
    private IUserRepo userRepo;
    private IEventBus eventBus;

    public UserService(IAuth0ApiClientBuilder tokenGenerator, IUserRepo userRepo, IEventBus eventBus) {
        this.tokenGenerator = tokenGenerator;
        this.userRepo = userRepo;
        this.eventBus = eventBus;
    }

    public async Task<User?> GetUserByAuth0Id(string auth0Id) {
        var managementApiClient = await tokenGenerator.GetManagementApiClient();

        var auth0User = await managementApiClient.Users.GetAsync(auth0Id);

        if (auth0User == null) {
            throw new EntityNotFoundException();
        }

        return await userRepo.FindByAuth0Id(auth0Id);
    }

    public async Task<User> GetOrCreateUserByAuth0Id(string auth0Id) {
        if (auth0Id == "") {
            throw new ArgumentException("Auth0 Id is missing.");
        }

        var managementApiClient = await tokenGenerator.GetManagementApiClient();

        var auth0User = await managementApiClient.Users.GetAsync(auth0Id);

        if (auth0User == null) {
            throw new EntityNotFoundException();
        }

        var user = await userRepo.FindByAuth0Id(auth0Id);

        if (user == null) {
            user = new User() {
                Id = Guid.NewGuid(),
                Auth0Id = auth0Id,
                Email = auth0User.Email
            };

            try {
                await userRepo.Add(user);
                await eventBus.Dispatch(new NewUserEvent(user));
            } catch (Exception) {
                // When multiple requests are fired off after the user registers it could lead to multiple users being created.
                Console.WriteLine("Duplicate user attempted to be created. Just use webhooks already");
            }

        }

        return user;
    }

    public async Task<User?> GetUserById(string id) {
        var user = await userRepo.FindById(new Guid(id));

        if (user == null) {
            return null;
        }

        var managementApiClient = await tokenGenerator.GetManagementApiClient();

        var auth0User = await managementApiClient.Users.GetAsync(user.Auth0Id);

        user.Email = auth0User.Email;
        return user;
    }
}