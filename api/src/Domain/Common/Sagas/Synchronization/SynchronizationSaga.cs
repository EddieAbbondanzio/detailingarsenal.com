namespace DetailingArsenal.Domain.Common {
    public class SynchronizationSaga : Saga {
        public SynchronizationSaga(
            RunDatabaseMigrationsStep runDatabaseMigrations,
            ValidateBillingConfigStep validateSubscriptionConfig,
            RefreshSubscriptionPlansStep refreshSubscriptionPlans,
            CreateOrUpdateAdminStep createOrUpdateAdmin,
            CreateRolesStep createProRoleStep,
            GiveAdminAllPermissionsStep giveAdminAllPermissions
            ) {
            Add(runDatabaseMigrations);
            Add(validateSubscriptionConfig);
            Add(refreshSubscriptionPlans);
            Add(createOrUpdateAdmin);
            Add(createProRoleStep);
            Add(giveAdminAllPermissions);
        }
    }
}