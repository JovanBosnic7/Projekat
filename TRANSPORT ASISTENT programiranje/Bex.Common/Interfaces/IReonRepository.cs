using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IReonRepository : IRepository<Reon>
    {
        IEnumerable<Reon> GetReonData();


        IEnumerable<Reon> GetSearchReonData(string searchTerms);

        int GetTotalReonData();

    }
}
