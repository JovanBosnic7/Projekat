

using System;
using System.Data.Entity.Infrastructure;


namespace Bex.DAL.EF.Common
{
    public interface ISaveChangesResolver
    {
        ICorrector GetCorrector(DbEntityEntry entityEntry);
        IValidator GetValidator(DbEntityEntry entityEntry);
    }
}
