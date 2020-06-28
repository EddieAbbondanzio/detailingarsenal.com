using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class NewUserInitializationSaga : Saga<User> {
        public NewUserInitializationSaga(
                            CreateBusinessStep createBusiness,
                            CreateHoursOfOperationStep createHoursOfOperation
                            ) {
            Add(createBusiness);
            Add(createHoursOfOperation);
        }
    }
}