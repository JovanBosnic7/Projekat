using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Bex.Models;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Bex.Common
{
    public class AddressRepository : IAddressRepository
    {
    
       
        public IQueryable<PAK> All => throw new NotImplementedException();

        public IQueryable<PAK> AllAsNoTracking => throw new NotImplementedException();

        public ObservableCollection<PAK> AllLocal => throw new NotImplementedException();

        public void Add(PAK entity, bool onlyChangeState = false)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<PAK> entities)
        {
            throw new NotImplementedException();
        }

        public PAK Find(Expression<Func<PAK, bool>> predicate, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public PAK Find(Expression<Func<PAK, bool>> predicate, bool asNoTracking = false, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public PAK Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PAK> GetAll(params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PAK> GetAll(bool asNoTracking = false, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PAK> GetAll(Expression<Func<PAK, bool>> predicate, bool asNoTracking = false, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Load<TElement>(PAK entity, params Expression<Func<PAK, ICollection<TElement>>>[] childrenProperties) where TElement : class
        {
            throw new NotImplementedException();
        }

        public void Load<TProperty>(Expression<Func<PAK, TProperty>> parentProperty, PAK entity) where TProperty : class
        {
            throw new NotImplementedException();
        }

        public void Remove(PAK entity, bool onlyChangeState = false)
        {
            throw new NotImplementedException();
        }

        public void Remove(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression<Func<PAK, bool>> predicate, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<PAK> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(PAK entity, bool getDatabaseValues = false)
        {
            throw new NotImplementedException();
        }
    
        //private AddressesDbContext context;

        //public AddressRepository(AddressesDbContext context)
        //{
        //    this.context = context;
        //}

        //public IEnumerable<PAK> GetPaks()
        //{
        //    return context.PAKs.ToList();
        //}

        //public PAK GetPakByID(int id)
        //{
        //    return context.PAKs.Find(id);
        //}

        //public void InsertPAk(PAK pak)
        //{
        //    context.PAKs.Add(pak);
        //}

        //public void DeletePAK(int pakID)
        //{
        //    PAK pak = context.PAKs.Find(pakID);
        //    context.PAKs.Remove(pak);
        //}

        //public void UpdatePak(PAK pak)
        //{
        //    context.Entry(pak).State = System.Data.Entity.EntityState.Modified;
        //}

        //public void Save()
        //{
        //    context.SaveChanges();
        //}

        //private bool disposed = false;

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            context.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //IEnumerable<PAK> IAddressRepository.GetPaks()
        //{
        //    throw new NotImplementedException();
        //}

        //PAK IAddressRepository.GetPakByID(int pakID)
        //{
        //    throw new NotImplementedException();
        //}

        //public void InsertPAk(PAK pak)
        //{
        //    throw new NotImplementedException();
        //}

        //public void UpdatePak(PAK pak)
        //{
        //    throw new NotImplementedException();
        //}
        public IQueryable<PAK> All => throw new NotImplementedException();

        public IQueryable<PAK> AllAsNoTracking => throw new NotImplementedException();

        public ObservableCollection<PAK> AllLocal => throw new NotImplementedException();

        public void Add(PAK entity, bool onlyChangeState = false)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<PAK> entities)
        {
            throw new NotImplementedException();
        }

        public PAK Find(Expression<Func<PAK, bool>> predicate, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public PAK Find(Expression<Func<PAK, bool>> predicate, bool asNoTracking = false, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public PAK Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PAK> GetAll(params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PAK> GetAll(bool asNoTracking = false, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PAK> GetAll(Expression<Func<PAK, bool>> predicate, bool asNoTracking = false, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void Load<TElement>(PAK entity, params Expression<Func<PAK, ICollection<TElement>>>[] childrenProperties) where TElement : class
        {
            throw new NotImplementedException();
        }

        public void Load<TProperty>(Expression<Func<PAK, TProperty>> parentProperty, PAK entity) where TProperty : class
        {
            throw new NotImplementedException();
        }

        public void Remove(PAK entity, bool onlyChangeState = false)
        {
            throw new NotImplementedException();
        }

        public void Remove(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void Remove(Expression<Func<PAK, bool>> predicate, params Expression<Func<PAK, object>>[] navigationProperties)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<PAK> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(PAK entity, bool getDatabaseValues = false)
        {
            throw new NotImplementedException();
        }
    }
}
