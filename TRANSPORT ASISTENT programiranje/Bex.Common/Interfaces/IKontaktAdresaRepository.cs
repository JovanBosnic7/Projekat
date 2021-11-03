using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IKontaktAdresaRepository : IRepository<KontaktAdresa>
    {
        IEnumerable<KontaktAdresa> GetKontaktAdresaData();     

    }
}
