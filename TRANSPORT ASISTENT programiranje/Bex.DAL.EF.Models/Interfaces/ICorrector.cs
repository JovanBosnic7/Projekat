using System;
using System.Data.Entity.Infrastructure;

namespace Bex.DAL.EF.Models
{
    public interface ICorrector
    {
        bool IsCorrected(DbEntityEntry entityEntry);
    }
}
