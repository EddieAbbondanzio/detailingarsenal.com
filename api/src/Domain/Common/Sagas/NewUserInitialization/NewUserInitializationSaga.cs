using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class NewUserInitializationSaga : Saga<User> {
        public NewUserInitializationSaga(
                            CreateBusinessStep createBusiness,
                            CreateHoursOfOperationStep createHoursOfOperation,
                            CreateTrialCustomerStep createTrialCustomer
                            ) {
            Add(createBusiness);
            Add(createHoursOfOperation);
            Add(createTrialCustomer);
        }
    }
}