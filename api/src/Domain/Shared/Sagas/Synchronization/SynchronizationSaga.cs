namespace DetailingArsenal.Domain {
    public class SynchronizationSaga : Saga {
        public SynchronizationSaga(
            RunDatabaseMigrationsStep runDatabaseMigrations,
            ValidateConfigFile validateSubscriptionConfig,
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