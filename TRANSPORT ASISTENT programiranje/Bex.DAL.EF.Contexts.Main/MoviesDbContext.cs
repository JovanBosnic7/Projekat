using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Demos.Club.DAL.EF.Common;
using Demos.Club.DAL.EF.Conventions;
using Demos.Club.DAL.EF.Models.Configurations;
using Demos.Club.Models;

namespace Demos.Club.DAL.EF.Contexts
{
    public partial class MoviesDbContext : BaseDbContext
    {
        static MoviesDbContext()
        {
            Database.SetInitializer(
                new NullDatabaseInitializer<MoviesDbContext>());
        }
        public MoviesDbContext() : this(new SaveChangesResolver())
        { }
        public MoviesDbContext(ISaveChangesResolver saveChangesResolver)
            : base(saveChangesResolver)
        { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieBinary> MovieBinaries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Properties<String>()
                .Where(p => p.Name == "Url")
                .Configure(p => p.HasMaxLength(1000));

            modelBuilder.Properties<String>()
                .Configure(p => p.HasMaxLength(150));

            modelBuilder.Properties<String>()
                .Where(p => p.Name == "Director")
                .Configure(p => p.HasMaxLength(50));

            modelBuilder.Configurations.Add(new MovieConfiguration());
            modelBuilder.Configurations.Add(new MovieBinaryConfiguration());

            // by convention, a type that has no primary key specified is treated
            //  as a complex type, but if type has a property called Id or similar
            modelBuilder.ComplexType<Distribution>();

            modelBuilder.Ignore<Post>();
        }

        public override int SaveChanges()
        {
            ShouldValidateOnSaveChanges = true;

            return base.SaveChanges();
        }
    }
}