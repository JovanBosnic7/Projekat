using Bex.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;


namespace Bex.DAL.EF.UOW
{
    public class BaseRepository<T> : IRepository<T>
        where T : class
    {
        public BaseRepository(DbContext dbContext)
        {
            if (dbContext == null)
            { throw new ArgumentNullException("DbContext is missing."); }

            DataContext = dbContext;
        }

        public virtual IQueryable<T> All =>
            DataSet;
        public virtual IQueryable<T> AllAsNoTracking =>
            DataSet.AsNoTracking();
        public virtual ObservableCollection<T> AllLocal =>
            DataSet.Local;

        public virtual IQueryable<T> GetAll(
            params Expression<Func<T, object>>[] navigationProperties)
        { return GetAll(false, navigationProperties); }

        public virtual IQueryable<T> GetAll(
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] navigationProperties)
        { return EntitiesQuery(null, asNoTracking, navigationProperties); }

        public virtual IQueryable<T> GetAll(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] navigationProperties)
        { return EntitiesQuery(predicate, asNoTracking, navigationProperties); }

        public virtual T Find(params object[] keyValues)
        { return DataSet.Find(keyValues); }

        public virtual T Find(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] navigationProperties)
        { return Find(predicate, false, navigationProperties); }

        public virtual T Find(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] navigationProperties)
        { return EntitiesQuery(predicate, asNoTracking, navigationProperties).SingleOrDefault(); }

        public virtual void Add(T entity, bool changeStateOnly = false)
        {
            if (entity == null)
            { return; }

            if (changeStateOnly)
            { EntityEntry(entity).State = EntityState.Added; }
            else
            { DataSet.Add(entity); }
        }

        public virtual void AddRange(IEnumerable<T> entities)
        {
            if (entities == null)
            { return; }

            DataSet.AddRange(entities);
        }

        public virtual void Update(T entity, bool getDatabaseValues = false)
        {
            if (entity == null)
            { return; }

            EntityEntry(entity).State = EntityState.Modified;

            if (getDatabaseValues)
            {
                var databaseValues = EntityEntry(entity).GetDatabaseValues();
                if (databaseValues != null)
                {
                    DataSet.Attach(entity); // = EntityState.Unchanged, clean properties
                    EntityEntry(entity).OriginalValues.SetValues(databaseValues);
                }
            }
        }

        public virtual void Remove(T entity, bool onlyChangeState = false)
        {
            if (entity == null)
            { return; }

            if (onlyChangeState)
            { EntityEntry(entity).State = EntityState.Deleted; }
            else
            {
                if (DataContext.Entry(entity).State == EntityState.Detached)
                { DataSet.Attach(entity); }

                DataSet.Remove(entity);
            }
        }

        public virtual void Remove(params object[] keyValues)
        {
            var entity = Find(keyValues);
            Remove(entity);
        }

        public virtual void Remove(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            RemoveRange(EntitiesQuery(predicate, navigationProperties));
        }

        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null)
            { return; }

            DataSet.RemoveRange(entities);
        }

        public virtual void Load<TElement>(
            T entity,
            params Expression<Func<T, ICollection<TElement>>>[] childrenProperties) where TElement : class
        {
            if (entity == null)
            { return; }

            foreach (var childrenProperty in childrenProperties)
            { ChildrenEntries(entity, childrenProperty).Load(); }
        }

        public virtual void Load<TProperty>(
            Expression<Func<T, TProperty>> parentProperty,
            T entity) where TProperty : class
        {
            if (entity == null)
            { return; }

            ParentEntry(entity, parentProperty).Load();
        }

        protected DbCollectionEntry<T, TElement> ChildrenEntries<TElement>(
            T entity,
            Expression<Func<T, ICollection<TElement>>> childrenProperty) where TElement : class
        {
            if (entity == null)
            { return null; }

            return EntityEntry(entity).Collection(childrenProperty);
        }

        protected DbReferenceEntry<T, TProperty> ParentEntry<TProperty>(
            T entity,
            Expression<Func<T, TProperty>> parentProperty) where TProperty : class
        {
            if (entity == null)
            { return null; }

            return EntityEntry(entity).Reference(parentProperty);
        }

        protected DbEntityEntry<T> EntityEntry(T entity)
        { return DataContext.Entry<T>(entity); }

        private IQueryable<T> EntitiesQuery(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] navigationProperties)
        { return EntitiesQuery(predicate, false, navigationProperties); }

        private IQueryable<T> EntitiesQuery(
            Expression<Func<T, bool>> predicate,
            bool asNoTracking = false,
            params Expression<Func<T, object>>[] navigationProperties)
        {
            var entitiesQuery = DataSet.AsQueryable();

            if (asNoTracking)
            { entitiesQuery = DataSet.AsNoTracking(); }

            if (predicate != null)
            { entitiesQuery = entitiesQuery.Where(predicate); }

            foreach (var navigationProperty in navigationProperties)
            { entitiesQuery = entitiesQuery.Include(navigationProperty); }

            return entitiesQuery;
        }

        protected DbContext DataContext { get; }
        protected DbSet<T> DataSet => DataContext.Set<T>();
    }
}
