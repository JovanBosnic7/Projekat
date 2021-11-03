using Bex.Common;
using Bex.DAL.EF.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;


namespace Bex.DAL.EF.Factory
{
    public class RepositoryProvider : IRepositoryProvider
    {
        public RepositoryProvider() :
            this(new FactoryProvider())
        { }
        public RepositoryProvider(IFactoryProvider factoryProvider) :
            this(factoryProvider, new Dictionary<Tuple<Type, Type>, object>())
        { }
        public RepositoryProvider(IDictionary<Tuple<Type, Type>, object> repositories) :
            this(new FactoryProvider(), repositories)
        { }
        public RepositoryProvider(
            IFactoryProvider factoryProvider,
            IDictionary<Tuple<Type, Type>, object> repositories)
        {
            FactoryProvider = factoryProvider;
            Repositories = repositories;
        }

        public DbContext DbContext { get; set; }

        public void BindRepository<TFrom, TTo>()
            where TTo : TFrom
            where TFrom : class
        {
            SetRepository(DbContext,
              (TFrom)Activator.CreateInstance(typeof(TTo), DbContext));
        }

        public T GetRepository<T>(Func<DbContext, object> factory = null)
                where T : class
        { return GetRepository<T>(DbContext, factory); }

        public T GetRepository<T>(DbContext dbContext, Func<DbContext, object> factory = null)
            where T : class
        {
            object repository;
            Repositories.TryGetValue(
                new Tuple<Type, Type>(typeof(T), dbContext.GetType()), out repository);

            if (repository != null)
            { return (T)repository; }

            return CreateRepository<T>(dbContext, factory);
        }

        public IRepository<T> GetEntityRepository<T>()
            where T : class
        {
            return GetRepository<IRepository<T>>(
               FactoryProvider.GetEntityRepositoryFactory<T>());
        }

        public IRepository<T> GetEntityRepository<T>(DbContext dbContext)
            where T : class
        {
            return GetRepository<IRepository<T>>(dbContext,
                FactoryProvider.GetEntityRepositoryFactory<T>());
        }

        public void SetRepository<T>(DbContext dbContext, T repository)
            where T : class
        {
            Repositories.Add(
                new Tuple<Type, Type>(typeof(T), dbContext.GetType()), repository);
        }

        private T CreateRepository<T>(
                    DbContext dbContext, Func<DbContext, object> factory)
                    where T : class
        {
            var f = factory ?? FactoryProvider.GetRepositoryFactory<T>();
            if (f == null)
            { throw new NullReferenceException($"No factory for type {typeof(T).FullName}"); }

            var repository = (T)f(dbContext);
            SetRepository(dbContext, repository);

            return repository;
        }

        IDictionary<Tuple<Type, Type>, object> Repositories { get; }
        private IFactoryProvider FactoryProvider { get; }
    }
}
