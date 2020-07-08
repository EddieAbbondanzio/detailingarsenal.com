namespace DetailingArsenal.Domain.Common {
    public class SynchronizationSaga : Saga {
        public SynchronizationSaga(
            RunDatabaseMigrationsStep runDatabaseMigrations,
            RefreshSubscriptionPlansStep refreshSubscriptionPlans,
            CreateOrUpdateAdminStep createOrUpdateAdmin,
            CreateProRoleStep createProRoleStep
            ) {
            Add(runDatabaseMigrations);
            Add(refreshSubscriptionPlans);
            Add(createOrUpdateAdmin);
            Add(createProRoleStep);
        }
    }
}