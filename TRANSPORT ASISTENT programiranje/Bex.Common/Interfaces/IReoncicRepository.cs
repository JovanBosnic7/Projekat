using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IReoncicRepository : IRepository<Reoncic>
    {
        IEnumerable<Reoncic> GetReoncicData();

        IEnumerable<Reoncic> GetSearchReoncicData(string searchTerms);

        int GetTotalReoncicData();
    }
}
