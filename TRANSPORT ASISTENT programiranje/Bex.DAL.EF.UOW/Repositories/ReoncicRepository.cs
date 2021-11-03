using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class ReoncicRepository : BaseRepository<Reoncic>, IReoncicRepository
    {
        public ReoncicRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public ReoncicRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        
        private IUowCommandResultFactory UowCommandResultFactory { get; }

        public IEnumerable<Reoncic> GetReoncicData()
        {
            var reoncic = DataSet.Include(p => p.Reon);
            return reoncic;
        }

        public IEnumerable<Reoncic> GetSearchReoncicData(string searchTerms)
        {
            var reoncicData = GetReoncicData().AsQueryable();

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnOznakaReona = "";
            string searchColumnNazivReoncica = "";
            string searchColumnPreuzimanjeDoDefault = "";
            bool searchColumnOdjavljujeSe = false;
            string searchColumnDatumPoslednjeOdjave = "";
            string searchColumnVremePoslednjeOdjave = "";
            bool searchColumnDeoMesta = false;
            string searchColumnNapomenaOdjava = "";


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
                        reoncicData = reoncicData.Where(k => k.Reon.OznReona.ToUpper().Contains(searchColumnOznakaReona.ToUpper()));

                    }
                    else if (searchColumn.Equals("NazivReoncica"))
                    {
                        searchColumnNazivReoncica = searchTxt;
                        reoncicData = reoncicData.Where(k => k.NazivReoncica.ToUpper().Contains(searchColumnNazivReoncica.ToUpper()));
                    }
                    else if (searchColumn.Equals("PreuzimanjeDoDefault"))
                    {
                        searchColumnPreuzimanjeDoDefault = searchTxt;
                        //reoncicData = reoncicData.Where(k => k.PreuzimanjeDoDefault == searchColumnPreuzimanjeDoDefault);
                    }
                    else if (searchColumn.Equals("OdjavljujeSe"))
                    {
                        searchColumnOdjavljujeSe = Boolean.Parse(searchTxt);
                        reoncicData = reoncicData.Where(k => k.OdjavljujeSe == searchColumnOdjavljujeSe);
                    }
                    else if (searchColumn.Equals("DatumPoslednjeOdjave"))
                    {
                        searchColumnDatumPoslednjeOdjave = searchTxt;
                        //reoncicData = reoncicData.Where(k => k.DatumPoslednjeOdjave.ToUpper().Contains(searchColumnBarkod.ToUpper()));
                    }
                    else if (searchColumn.Equals("VremePoslednjeOdjave"))
                    {
                        searchColumnVremePoslednjeOdjave = searchTxt;
                        //reoncicData = reoncicData.Where(k => k.VremePoslednjeOdjave == searchColumnVremePoslednjeOdjave);
                    }
                    else if (searchColumn.Equals("DeoMesta"))
                    {
                        searchColumnDeoMesta = Boolean.Parse(searchTxt);
                        reoncicData = reoncicData.Where(k => k.DeoMesta == searchColumnDeoMesta);
                    }
                    else if (searchColumn.Equals("NapomenaOdjava"))
                    {
                        searchColumnNapomenaOdjava = searchTxt;
                        reoncicData = reoncicData.Where(k => k.NapomenaOdjave.ToUpper().Contains(searchColumnNapomenaOdjava.ToUpper()));
                    }

                }

            }

            return reoncicData;
        }

        public int GetTotalReoncicData()
        {
            var reoncicQuery = DataSet.AsQueryable();

            return reoncicQuery.Count();
        }
    }
}
