using DetailingArsenal.Domain.Users;

namespace DetailingArsenal.Domain.Common {
    public class NewUserSaga : Saga<User> {
        public NewUserSaga(
                            CreateBusiness createBusiness,
                            CreateHoursOfOperation createHoursOfOperation
                            ) {
            Add(createBusiness);
            Add(createHoursOfOperation);
        }
    }
}