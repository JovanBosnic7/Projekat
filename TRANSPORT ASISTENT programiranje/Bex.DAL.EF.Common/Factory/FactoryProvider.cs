using Bex.DAL.EF.Common;
using Bex.DAL.EF.UOW;
using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace Bex.DAL.EF.Factory
{
    public class FactoryProvider : IFactoryProvider
    {
        public FactoryProvider()
            : this(new Dictionary<Type, Func<DbContext, object>>())
        { }
        public FactoryProvider(
            IDictionary<Type, Func<DbContext, object>> factories)
        {
            Factories = factories;
        }

        public Func<DbContext, object> GetRepositoryFactory<T>()
            where T : class
        {
            Func<DbContext, object> factory;
            Factories.TryGetValue(typeof(T), out factory);

            return factory;
        }

        public Func<DbContext, object> GetEntityRepositoryFactory<T>()
            where T : class
        { return dbContext => new BaseRepository<T>(dbContext); }

        private IDictionary<Type, Func<DbContext, object>> Factories { get; }
    }
}
