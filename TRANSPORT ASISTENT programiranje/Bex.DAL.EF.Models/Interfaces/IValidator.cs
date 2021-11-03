using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace Bex.DAL.EF.Models
{
    public interface IValidator
    {
        bool ShouldValidate(DbEntityEntry entityEntry);
        DbEntityValidationResult Validate(
            DbEntityEntry entityEntry,
            IDictionary<object, object> items,
            Func<DbEntityEntry, IDictionary<object, object>, DbEntityValidationResult> baseValidateEntity);
    }
}
