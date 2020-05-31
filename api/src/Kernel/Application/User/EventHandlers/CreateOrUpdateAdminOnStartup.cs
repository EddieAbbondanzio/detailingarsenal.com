using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Serilog;

/// <summary>
/// When a new user is generated, go ahead and email Ed.
/// </summary>
public class CreateOrUpdateAdminOnStartup : IBusEventHandler<StartupEvent> {
    private AdminConfig config;
    private IUserService userService;

    public CreateOrUpdateAdminOnStartup(AdminConfig config, IUserService userService) {
        this.config = config;
        this.userService = userService;
    }

    public async Task Handle(StartupEvent busEvent) {
        var user = await userService.GetUserByEmail(config.Email);

        if (user != null) {
            await userService.UpdatePassword(user, config.Password);
            Log.Information("Updated admin password");
        } else {
            user = await userService.CreateUser(config.Email, config.Password);
            Log.Information("Created new admin user");
        }
    }
}