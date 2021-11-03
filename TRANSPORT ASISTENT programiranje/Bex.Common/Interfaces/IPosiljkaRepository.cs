using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IPosiljkaRepository : IRepository<Posiljka>
    {
        IEnumerable<Posiljka> GetPosiljkaData();

        //IEnumerable<Kontakt> GetSearchKontaktData(string searchTxt);

        IEnumerable<Posiljka> GetSearchPosiljkaData(string searchTerms);

        int GetTotalPosiljka();
    }
}
