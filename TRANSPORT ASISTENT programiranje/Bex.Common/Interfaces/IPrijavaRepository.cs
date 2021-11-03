using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IPrijavaRepository : IRepository<PrijavaReklamacijaZalba>
    {
        IEnumerable<PrijavaReklamacijaZalba> GetPrijavaData();

        
        int GetTotalPrijava();
    }
}
