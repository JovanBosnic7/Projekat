using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class ReonRepository : BaseRepository<Reon>, IReonRepository
    {
        public ReonRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public ReonRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        
        private IUowCommandResultFactory UowCommandResultFactory { get; }

        public IEnumerable<Reon> GetReonData()
        {
            var reon = DataSet.Include(p => p.Region)
                              .Include(t => t.ReonTip);
            return reon;
        }

        

        public IEnumerable<Reon> GetSearchReonData(string searchTerms)
        {
            var reonData = GetReonData().AsQueryable();

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnOznakaReona = "";
            string searchColumnNazivReona = "";
            string searchColumnRegion = "";
            int searchColumnTip = 0;
            string searchColumnBarkod = "";
            int searchColumnKm = 0;
            //string searchColumnStorno = "";


            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {

                    if (searchColumn.Equals("OznakaReona"))
                    {
                        searchColumnOznakaReona = searchTxt;
                        reonData = reonData.Where(k => k.OznReona.ToUpper().Contains(searchColumnOznakaReona.ToUpper()));

                    }
                    else if (searchColumn.Equals("NazivReona"))
                    {
                        searchColumnNazivReona = searchTxt;
                        reonData = reonData.Where(k => k.NazivReona.ToUpper().Contains(searchColumnNazivReona.ToUpper()));
                    }
                    else if (searchColumn.Equals("NazivRegiona"))
                    {
                        searchColumnRegion = searchTxt;
                        reonData = reonData.Where(k => k.Region.NazivSkraceni.ToUpper().Contains(searchColumnRegion.ToUpper()));
                    }
                    else if (searchColumn.Equals("TipReona"))
                    {
                        searchColumnTip = int.Parse(searchTxt);
                        reonData = reonData.Where(k => k.Tip == searchColumnTip);
                    }
                    else if (searchColumn.Equals("BarkodReona"))
                    {
                        searchColumnBarkod = searchTxt;
                        reonData = reonData.Where(k => k.BarKodReona.ToUpper().Contains(searchColumnBarkod.ToUpper()));
                    }
                    else if (searchColumn.Equals("KmReona"))
                    {
                        searchColumnKm = int.Parse(searchTxt);
                        reonData = reonData.Where(k => k.KmDoReona == searchColumnKm);
                    }

                }

            }

            return reonData;
        }

        public int GetTotalReonData()
        {
            var reonQuery = DataSet.AsQueryable();

            return reonQuery.Count();
        }
    }
}
