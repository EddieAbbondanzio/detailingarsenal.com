namespace DetailingArsenal.Domain.Common {
    public class SynchronizationSaga : Saga {
        public SynchronizationSaga(
            RunDatabaseMigrations runDatabaseMigrationsStep,
            RefreshSubscriptionPlans refreshSubscriptionPlans,
            CreateOrUpdateAdmin createOrUpdateAdmin
            ) {
            Add(createOrUpdateAdmin);
            Add(refreshSubscriptionPlans);
            Add(createOrUpdateAdmin);
        }
    }
}