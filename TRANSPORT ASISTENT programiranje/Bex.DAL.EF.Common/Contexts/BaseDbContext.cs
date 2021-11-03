using AspNet.DAL.EF.Models.Security;
using Bex.DAL.EF.Common;
using Bex.DAL.EF.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;


namespace Bex.DAL.EF.Contexts
{
    [DbConfigurationType(typeof(ContextsConfiguration))]
    public class BaseDbContext : DbContext
    {
        
        public BaseDbContext(ISaveChangesResolver saveChangesResolver)
                    : base()
        {
            SaveChangesResolver = saveChangesResolver;
            Configuration.ValidateOnSaveEnabled = false;
            ShouldValidateOnSaveChanges = true;
        }
        public BaseDbContext(
            DbConnection dbConnection,
            bool contextOwnsConnection,
            ISaveChangesResolver saveChangesResolver)
            : base(dbConnection, contextOwnsConnection)
        {
            SaveChangesResolver = saveChangesResolver;
            Configuration.ValidateOnSaveEnabled = false;
            ShouldValidateOnSaveChanges = true;
        }
        public BaseDbContext(string name, ISaveChangesResolver saveChangesResolver)
            : base(name)
        {
            SaveChangesResolver = saveChangesResolver;
            Configuration.ValidateOnSaveEnabled = false;
            ShouldValidateOnSaveChanges = true;
        }

        protected bool ShouldValidateOnSaveChanges { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions
            //    .Remove<ColumnOrderingConventionStrict>();

            modelBuilder.Conventions
                .Add(new IdOrKeyKeyDiscoveryConvention());
        }


        protected override bool ShouldValidateEntity(DbEntityEntry entityEntry)
        {
            return
                SaveChangesResolver?.GetValidator(entityEntry)
                    ?.ShouldValidate(entityEntry) ??
                base.ShouldValidateEntity(entityEntry);
        }

        protected override DbEntityValidationResult ValidateEntity(
            DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            return
                SaveChangesResolver?.GetValidator(entityEntry)?.Validate(
                    entityEntry, items, (e, i) => base.ValidateEntity(e, i)) ??
                base.ValidateEntity(entityEntry, items);
        }

        public override int SaveChanges()
        {
            var autoDetectChangesEnabled = Configuration.AutoDetectChangesEnabled;

            try
            {
                Configuration.AutoDetectChangesEnabled = false;
                bool isSomethingCorrected = false;

                foreach (var entry in ChangeTracker.Entries())
                {
                    isSomethingCorrected |= SaveChangesResolver
                        ?.GetCorrector(entry)?.IsCorrected(entry) ?? false;
                }

                if (isSomethingCorrected)
                { ChangeTracker.DetectChanges(); }

                if (ShouldValidateOnSaveChanges)
                {
                    var dbEntityValidationResults = GetValidationErrors();
                    if (dbEntityValidationResults.Any())
                    {
                        throw new DbEntityValidationException(
                            "Validation errors are found in DbContext.Save() method",
                            dbEntityValidationResults);
                    }
                }

                return base.SaveChanges();
            }
            finally
            { Configuration.AutoDetectChangesEnabled = autoDetectChangesEnabled; }
        }

        protected ISaveChangesResolver SaveChangesResolver { get; }
    }
}