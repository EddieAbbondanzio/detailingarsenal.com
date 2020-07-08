namespace DetailingArsenal.Domain.Common {
    public class SynchronizationSaga : Saga {
        public SynchronizationSaga(
            RunDatabaseMigrationsStep runDatabaseMigrations,
            ValidateSubscriptionConfigStep validateSubscriptionConfig,
            RefreshSubscriptionPlansStep refreshSubscriptionPlans,
            CreateOrUpdateAdminStep createOrUpdateAdmin,
            CreateProRoleStep createProRoleStep
            ) {
            Add(runDatabaseMigrations);
            Add(validateSubscriptionConfig);
            Add(refreshSubscriptionPlans);
            Add(createOrUpdateAdmin);
            Add(createProRoleStep);
        }
    }
}