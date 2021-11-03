using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface ICenovnikRepository : IRepository<Cenovnik>
    {
        IEnumerable<Cenovnik> GetCenovnikData();
        IEnumerable<Cenovnik> GetSearchCenovnikData(string searchTerms);
        IEnumerable<Cenovnik> GetCenovnikAutocompleteData(string searchQuery);
        int GetTotalCenovnik();

    }
}
