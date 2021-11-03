using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IBankaRepository : IRepository<BankaPazara>
    {
        IEnumerable<BankaPazara> GetBankaData();
        IEnumerable<BankaPazara> GetSearchBankaData(string searchTerms);
        int GetTotalBanka();

    }
}
