using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IZaposleniRepository : IRepository<Zaposleni>
    {

        IEnumerable<Zaposleni> GetZaposleniAutocompleteData(string searchQuery);
        IEnumerable<Zaposleni> GetZaposleniPoStaromAutocompleteData(string searchQuery); //moze da se izbrise kad sve stete predju na novi program

    }
}
