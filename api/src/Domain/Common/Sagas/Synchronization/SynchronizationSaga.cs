namespace DetailingArsenal.Domain.Common {
    public class SynchronizationSaga : Saga {
        public SynchronizationSaga(
            RunDatabaseMigrationsStep runDatabaseMigrations,
            RefreshSubscriptionPlansStep refreshSubscriptionPlans,
            CreateOrUpdateAdminStep createOrUpdateAdmin
            ) {
            Add(runDatabaseMigrations);
            Add(refreshSubscriptionPlans);
            Add(createOrUpdateAdmin);
        }
    }
}