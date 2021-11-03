using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;


namespace Bex.DAL.EF.Contexts
{
    public class ContextsConfiguration : DbConfiguration
    {
        public ContextsConfiguration()
        {
            SetProviderServices(
                SqlProviderServices.ProviderInvariantName,
                SqlProviderServices.Instance);

            SetProviderServices(
                "Oracle.ManagedDataAccess.Client",
                Oracle.ManagedDataAccess.EntityFramework.EFOracleProviderServices.Instance);

            SetDatabaseLogFormatter(
                (context, writeAction) => new Logging.SqlLoggerFormatter(context, writeAction));
        }
    }
}
