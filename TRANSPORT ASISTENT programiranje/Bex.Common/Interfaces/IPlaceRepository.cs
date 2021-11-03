using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IPlaceRepository : IRepository<Place>
    {
        IEnumerable<Place> GetPlaceData();

        IEnumerable<Place> GetSearchPlaceData(string searchTerms);

        int GetTotalPlaceData();
    }
}
