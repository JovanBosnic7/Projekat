using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface ITPterminiRepository : IRepository<TPtermini>
    {
        IUowCommandResult ZakljuciDan(decimal? iznosKes = 0, decimal? iznosKartica = 0, decimal? iznosCek = 0, int? lokacijaId = 0, int? userId = 0);

    }
}
