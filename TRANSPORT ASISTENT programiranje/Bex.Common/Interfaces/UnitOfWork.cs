using EFproject.Models;
using System;


namespace EFproject.DAL
{
    public class UnitOfWork : IDisposable
    {
        private AddressesDbContext context = new AddressesDbContext();
       // private GenericRepository<Department> departmentRepository;
      //  private CourseRepository courseRepository;

        /*public GenericRepository<Department> DepartmentRepository
        {
            get
            {

                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new GenericRepository<Department>(context);
                }
                return departmentRepository;
            }
        }
        
        public CourseRepository CourseRepository
        {
            get
            {

                if (this.courseRepository == null)
                {
                    this.courseRepository = new CourseRepository(context);
                }
                return courseRepository;
            }
        }*/


        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
