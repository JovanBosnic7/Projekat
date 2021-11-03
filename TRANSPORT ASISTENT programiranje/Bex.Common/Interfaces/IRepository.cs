using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> All { get; }
        IQueryable<T> AllAsNoTracking { get; }
        ObservableCollection<T> AllLocal { get; }

        IQueryable<T> GetAll(
            params Expression<Func<T, object>>[] navigationProperties);
        IQueryable<T> GetAll(
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] navigationProperties);
        IQueryable<T> GetAll(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] navigationProperties);
        T Find(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] navigationProperties);
        T Find(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] navigationProperties);
        T Find(params object[] keyValues);
        void Add(T entity, bool onlyChangeState = false);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity, bool getDatabaseValues = false);
        void Remove(T entity, bool onlyChangeState = false);
        void Remove(params object[] keyValues);
        void Remove(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] navigationProperties);
        void RemoveRange(IEnumerable<T> entities);
        void Load<TElement>(
            T entity,
            params Expression<Func<T, ICollection<TElement>>>[] childrenProperties) where TElement : class;
        void Load<TProperty>(
            Expression<Func<T, TProperty>> parentProperty,
            T entity) where TProperty : class;
    }
}
