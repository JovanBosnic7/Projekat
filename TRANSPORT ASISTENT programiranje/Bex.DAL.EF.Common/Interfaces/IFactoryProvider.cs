using System;
using System.Data.Entity;

namespace Bex.DAL.EF.Common
{
    public interface IFactoryProvider
    {
        Func<DbContext, object> GetRepositoryFactory<T>()
            where T : class;
        Func<DbContext, object> GetEntityRepositoryFactory<T>()
            where T : class;
    }
}
