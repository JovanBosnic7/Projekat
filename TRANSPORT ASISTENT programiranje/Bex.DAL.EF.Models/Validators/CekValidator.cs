using Bex.DAL.EF.Models;
using Bex.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;


namespace Bex.DAL.EF.Models.Validators
{
    public class KontaktValidator : IValidator
    {
        public bool ShouldValidate(DbEntityEntry entityEntry)
        {
            return
                entityEntry.State == EntityState.Added ||
                entityEntry.State == EntityState.Modified;
        }

        public DbEntityValidationResult Validate(
            DbEntityEntry entityEntry,
            IDictionary<object, object> items,
            Func<DbEntityEntry, IDictionary<object, object>, DbEntityValidationResult> baseValidateEntity)
        {
            var kontakt = (Kontakt)entityEntry.Entity;

            var dbValidationErrors = new List<DbValidationError>();

            if (dbValidationErrors.Count == 0)
            {
                if (String.Equals(kontakt.Prezime, kontakt.Ime))
                {
                    items.Add(
                        "MovieTitle_IsEqualTo_MovieDirector",
                        $"Title (Current): {kontakt.Prezime}, Director (Current): {kontakt.Ime}, " +
                            $"Director (Original): {entityEntry.Property("Director").OriginalValue}.");
                }

                dbValidationErrors.AddRange(
                    baseValidateEntity(entityEntry, items).ValidationErrors);
            }

            return new DbEntityValidationResult(entityEntry, dbValidationErrors);
        }
    }
}
