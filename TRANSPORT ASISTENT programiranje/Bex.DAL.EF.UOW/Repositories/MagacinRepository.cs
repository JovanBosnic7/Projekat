using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class MagacinRepository : BaseRepository<KurirZaduzenje>, IMagacinRepository
    {
        public MagacinRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public MagacinRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        
 
        private IUowCommandResultFactory UowCommandResultFactory { get; }

        public int GetTotalZaduzenja()
        {
            var zaduzenjaQuery = DataSet.AsQueryable().Where(x=>x.Reon.RegionId == 2);

            return zaduzenjaQuery.Count();
        }

        public IEnumerable<KurirZaduzenje> GetZaduzenjaKurira()
        {
            var zaduzenja = DataSet.Include(g => g.Reon)
            //.Include(r => r.User)
            .Include(s => s.Zona);
            return zaduzenja;
        }

        public IUowCommandResult KreirajZbirneOtkupe(List<string> listaRegiona)
        {
            foreach(var reg in listaRegiona)
            {
                var sqlString = @"EXEC prKreirajZbirne " + reg; // ??? proveriti da li je potrebno da ovde stoji reg ako se prosledjuje dole

                return UowCommandResultFactory.Invoke(() =>
                {
                    return DataContext.Database.ExecuteSqlCommand(sqlString, reg);
                });
            }

            return null;
        }


    }
}
