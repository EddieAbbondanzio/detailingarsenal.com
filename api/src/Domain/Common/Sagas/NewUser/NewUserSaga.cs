using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class NewUserSaga : Saga<string> {
        public NewUserSaga(
                            CreateUserStep createUser,
                            CreateBusinessStep createBusiness,
                            CreateHoursOfOperationStep createHoursOfOperation,
                            CreateTrialCustomerStep createTrialCustomer
                            ) {
            Add(createUser);
            Add(createBusiness);
            Add(createHoursOfOperation);
            Add(createTrialCustomer);
        }
    }
}