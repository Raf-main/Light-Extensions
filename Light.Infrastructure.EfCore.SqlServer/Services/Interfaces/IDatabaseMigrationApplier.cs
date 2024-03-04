namespace Light.Infrastructure.EfCore.Services.Interfaces
{
    public interface IDatabaseMigrationApplier
    {
        void ApplyMigrations();
    }
}
