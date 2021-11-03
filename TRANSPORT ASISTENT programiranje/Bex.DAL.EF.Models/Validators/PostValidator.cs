using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using Demos.Club.DAL.EF.Models.Common;
using Demos.Club.Models;

namespace Demos.Club.DAL.EF.Models.Validators
{
    public class PostValidator : IValidator
    {
        public bool ShouldValidate(DbEntityEntry entityEntry)
        {
            return
                entityEntry.State == EntityState.Added ||
                entityEntry.State == EntityState.Modified ||
                entityEntry.State == EntityState.Deleted;
        }

        public DbEntityValidationResult Validate(
            DbEntityEntry entityEntry,
            IDictionary<object, object> items,
            Func<DbEntityEntry, IDictionary<object, object>, DbEntityValidationResult> baseValidateEntity)
        {
            var post = (Post)entityEntry.Entity;

            var validationResult = baseValidateEntity(entityEntry, items);

            if (entityEntry.State == EntityState.Deleted)
            {
                validationResult = new DbEntityValidationResult(
                    entityEntry,
                    new List<DbValidationError>());

                if (post.Key < 5)
                {
                    validationResult.ValidationErrors.Add(
                        new DbValidationError(
                            "",
                            "Post with Id smaller than 7 can't be deleted."));
                }
            }

            return validationResult;
        }
    }
}
