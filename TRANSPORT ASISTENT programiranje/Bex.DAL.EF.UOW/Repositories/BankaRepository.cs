using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class BankaRepository : BaseRepository<BankaPazara>, IBankaRepository
    {
        public BankaRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public BankaRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }

        public IEnumerable<Banka> GetBankaData()
        {
            var banka = DataSet.Include(r => r.Region);

            return banka;
        }
        public IEnumerable<Banka> GetSearchBankaData(string searchTerms)

        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";


            string region = "";
            string datum = "";


            var banka = GetBankaData().AsQueryable();



            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];



                if (searchColumn.Equals("Datum") && !String.IsNullOrEmpty(searchTxt))
                {
                    string[] arrDatum = searchTxt.Split('/');
                    DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    banka = banka.Where(k => k.Datum == thisDate1);
                }
                else if (searchColumn.Equals("Region") && !String.IsNullOrEmpty(searchTxt))
                {
                    region = searchTxt;
                    banka = banka.Where(k => k.Region.NazivSkraceni.ToUpper().Contains(region.ToUpper()));
                }

            }
            return banka;
        }

        private IUowCommandResultFactory UowCommandResultFactory { get; }

        public int GetTotalBanka()
        {
            var bankaQuery = DataSet.AsQueryable().Where(x => x.RegionId == 2);

            return bankaQuery.Count();
        }
    }
}
