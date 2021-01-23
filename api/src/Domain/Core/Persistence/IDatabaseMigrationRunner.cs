namespace DetailingArsenal.Domain {
    public interface IDatabaseMigrationRunner {
        public abstract void MigrateUp();
        public abstract void MigrateDown(long version);
    }
}