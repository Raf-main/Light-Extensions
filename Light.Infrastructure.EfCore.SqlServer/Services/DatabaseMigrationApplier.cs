using Light.Infrastructure.EfCore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Light.Infrastructure.EfCore.Services
{
    public class DatabaseMigrationApplier(DbContext context) : IDatabaseMigrationApplier
    {
        public void ApplyMigrations()
        {
            
            context.Database.Migrate();
        }
    }
}
