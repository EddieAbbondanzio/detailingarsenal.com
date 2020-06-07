using System;
using System.Threading.Tasks;

public class CreateCustomerAndStartTrialOnNewUser : IBusEventHandler<NewUserEvent> {
    private ICustomerRepo repo;

    public CreateCustomerAndStartTrialOnNewUser(ICustomerRepo repo) {
        this.repo = repo;
    }

    public async Task Handle(NewUserEvent busEvent) {
        // Create the customer
        var customer = new Customer() {
            Info = new CustomerInfo(null!, busEvent.User.Email)
        };

        await repo.Add(customer);

        // Now create subscription
    }
}