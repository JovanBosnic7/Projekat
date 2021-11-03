using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IUgovorRepository : IRepository<Ugovor>
    {
        IEnumerable<Ugovor> GetUgovorData();
        IEnumerable<Ugovor> GetSearchUgovorData(string searchTerms);
        int GetTotalUgovor();

    }
}
