using Bex.Common;
using System;
using System.Data.Entity;


namespace Bex.DAL.EF.Common
{
    public interface IRepositoryProvider
    {
        DbContext DbContext { get; set; }

        void BindRepository<TFrom, TTo>()
            where TTo : TFrom
            where TFrom : class;
        IRepository<T> GetEntityRepository<T>()
            where T : class;
        IRepository<T> GetEntityRepository<T>(DbContext dbContext)
            where T : class;
        T GetRepository<T>(Func<DbContext, object> factory = null)
            where T : class;
        T GetRepository<T>(DbContext dbContext, Func<DbContext, object> factory = null)
            where T : class;
        void SetRepository<T>(DbContext dbContext, T repository)
            where T : class;
    }
}
