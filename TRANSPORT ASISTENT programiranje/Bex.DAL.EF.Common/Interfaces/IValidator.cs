using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace Bex.DAL.EF.Common
{
    public interface IValidator
    {
        bool ShouldValidate(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry);
        System.Data.Entity.Validation.DbEntityValidationResult Validate(
            DbEntityEntry entityEntry,
            IDictionary<object, object> items,
            Func<DbEntityEntry, IDictionary<object, object>, DbEntityValidationResult> baseValidateEntity);
    }
}