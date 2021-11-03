using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IKontaktRepository : IRepository<Kontakt>
    {
        IEnumerable<Kontakt> GetKontaktData();

        //IEnumerable<Kontakt> GetSearchKontaktData(string searchTxt);

        IEnumerable<Kontakt> GetSearchKontaktDataRepository(string searchTerms);
        IEnumerable<Kontakt> GetKontaktAutocompliteData(string searchQuery, bool isPravno, bool isAll);

        int GetTotalKontakts();
    }
}
