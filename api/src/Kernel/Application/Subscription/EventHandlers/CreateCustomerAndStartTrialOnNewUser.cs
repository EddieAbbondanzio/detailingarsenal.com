using System;
using System.Threading.Tasks;
using Serilog;

public class CreateCustomerAndStartTrialOnNewUser : IBusEventHandler<NewUserEvent> {
    private ICustomerRepo repo;
    private ISubscriptionService subscriptionService;

    public CreateCustomerAndStartTrialOnNewUser(ICustomerRepo repo, ISubscriptionService subscriptionService) {
        this.repo = repo;
        this.subscriptionService = subscriptionService;
    }

    public async Task Handle(NewUserEvent busEvent) {
        // Create the customer
        var customer = new Customer() {
            Id = Guid.NewGuid(),
            Info = new CustomerInfo(null!, busEvent.User.Email)
        };

        try {
            await repo.Add(customer);

        } catch (Exception e) {
            Log.Fatal(e, "REEE");
        }

        // Now create and start the trial subscription
        var sub = await subscriptionService.CreateTrialSubscription(customer);
    }
}