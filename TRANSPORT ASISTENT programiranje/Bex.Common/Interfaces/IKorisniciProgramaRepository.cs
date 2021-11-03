using Bex.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bex.Common
{
    public interface IKorisniciProgramaRepository : IRepository<KorisniciPrograma>
    {
        IEnumerable<KorisniciPrograma> GetUserData();
        IEnumerable<KorisniciPrograma> GetSearchUserData(string searchTerms);
        int GetTotalUser();

        IEnumerable<KorisniciPrograma> GetKorisniciProgramaAutocompliteData(string searchQuery);



    }
}
