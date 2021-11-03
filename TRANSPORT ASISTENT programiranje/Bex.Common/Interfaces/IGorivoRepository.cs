using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IGorivoRepository : IRepository<GorivoTocenje>
    {
        IUowCommandResult GorivoImport(string registracija="", decimal kolicina = 0, int kilometraza = 0, decimal cena = 0, string vreme="", DateTime? datum = null,int pumpaId=0);

    }
}
