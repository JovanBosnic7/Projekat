using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IStreetRepository : IRepository<Street>
    {
        IEnumerable<Street> GetStreetData();
        IEnumerable<Street> GetSearchStreetData(string searchTerms);

        int GetTotalStreetData();


    }
}
