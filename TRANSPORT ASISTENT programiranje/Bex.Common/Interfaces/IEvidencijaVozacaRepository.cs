using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IEvidencijaVozacaRepository : IRepository<EvidencijaVozaca>
    {
        IUowCommandResult EvidencijaImport(int zaposleniId, string dan, DateTime datum, string start, string stop, string radniDan, string dnevnoRadnoVreme, string upravljanje, string ostalo, string raspolozivost, string odmor, string odsustvo, string noc);

    }
}
