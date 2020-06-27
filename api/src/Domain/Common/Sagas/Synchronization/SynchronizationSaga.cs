namespace DetailingArsenal.Domain.Common {
    public class SynchronizationSaga : Saga {
        public SynchronizationSaga(
            RunDatabaseMigrations runDatabaseMigrations,
            RefreshSubscriptionPlans refreshSubscriptionPlans,
            CreateOrUpdateAdmin createOrUpdateAdmin
            ) {
            Add(runDatabaseMigrations);
            Add(refreshSubscriptionPlans);
            Add(createOrUpdateAdmin);
        }
    }
}