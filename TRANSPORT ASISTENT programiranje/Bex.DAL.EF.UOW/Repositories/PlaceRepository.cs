using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
    {
        public PlaceRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public PlaceRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        
        private IUowCommandResultFactory UowCommandResultFactory { get; }

        public IEnumerable<Place> GetPlaceData()
        {
            var place = DataSet.Include(p => p.Opstina);
            return place;
        }

        public IEnumerable<Place> GetSearchPlaceData(string searchTerms)
        {
            var placeData = DataSet.Include(p => p.Opstina).AsQueryable();

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnMesto = "";
            string searchColumnOpstina = "";
            string searchColumnPtt = "";


            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {

                    if (searchColumn.Equals("NazivMesta"))
                    {
                        searchColumnMesto = searchTxt;
                        placeData = placeData.Where(k => k.PlaceName.ToUpper().Contains(searchColumnMesto.ToUpper()));

                    }
                    else if (searchColumn.Equals("NazivOpstine"))
                    {
                        searchColumnOpstina = searchTxt;
                        placeData = placeData.Where(k => k.Opstina.OpstinaNaziv.ToUpper().Contains(searchColumnOpstina.ToUpper()));
                    }
                    else if (searchColumn.Equals("Ptt"))
                    {
                        searchColumnPtt = searchTxt;
                        placeData = placeData.Where(k => k.Ptt.ToUpper().Contains(searchColumnPtt.ToUpper()));
                    }

                }

            }

            return placeData;
        }

        public int GetTotalPlaceData()
        {
            var placeQuery = DataSet.AsQueryable();

            return placeQuery.Count();
        }
    }
}
