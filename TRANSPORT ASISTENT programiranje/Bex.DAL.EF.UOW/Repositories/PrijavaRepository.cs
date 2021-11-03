using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class PrijavaRepository : BaseRepository<PrijavaReklamacijaZalba>, IPrijavaRepository
    {
        public PrijavaRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public PrijavaRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        public IEnumerable<PrijavaReklamacijaZalba> GetPrijavaData()
        {
            var prijava = DataSet.Include(k => k.PrijavaNacin)
            .Include(r => r.PrijavaTip)
            .Include(s => s.PrijavaPodTip)
            .Include(f => f.PrijavaStatus)
            .Include(u => u.KorisniciPrograma.Kontakt);

            return prijava;
        }

        public int GetTotalPrijava()
        {
            var prijavaQuery = DataSet.AsQueryable();

            return prijavaQuery.Count();
        }

        

        private IUowCommandResultFactory UowCommandResultFactory { get; }
    }
}
