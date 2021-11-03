using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class ZaposleniRepository : BaseRepository<Zaposleni>, IZaposleniRepository
    {
        public ZaposleniRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public ZaposleniRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }


        public IEnumerable<Zaposleni> GetZaposleniAutocompleteData(string searchQuery)
        {
            var zaposleniData = DataSet
                                    // ?.Include(k => k.Kontakt)
                                     //?.Include(m => m.ZaposleniRadnoMesto)
                                     .Where(a => a.Aktivan == true)
                                     //.Where(x => (x.Kontakt.Naziv.ToUpper()).Contains(searchQuery.ToUpper()))
                                     .AsEnumerable();


            return zaposleniData;
        }

        public IEnumerable<Zaposleni> GetZaposleniPoStaromAutocompleteData(string searchQuery)
        {
            var zaposleniData = DataSet
                                     //?.Include(k => k.Kontakt)
                                     //?.Include(m => m.ZaposleniRadnoMesto)
                                     .Where(a => a.Aktivan == true)
                                     //.Where(x => (x.Kontakt.Naziv.ToUpper()).Contains(searchQuery.ToUpper()))
                                     .AsEnumerable();


            return zaposleniData;
        }
        private IUowCommandResultFactory UowCommandResultFactory { get; }
    }
}
