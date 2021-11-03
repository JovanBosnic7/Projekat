using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class StreetRepository : BaseRepository<Street>, IStreetRepository
    {
        public StreetRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public StreetRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        
        private IUowCommandResultFactory UowCommandResultFactory { get; }

        public IEnumerable<Street> GetSearchStreetData(string searchTerms)
        {
            var streetData = DataSet.Include(p => p.Place).AsQueryable();

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnMesto = "";
            string searchColumnUlica = "";


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
                        streetData = streetData.Where(k => k.Place.PlaceName.ToUpper().Contains(searchColumnMesto.ToUpper()));

                    }
                    else if (searchColumn.Equals("NazivUlice"))
                    {
                        searchColumnUlica = searchTxt;
                        streetData = streetData.Where(k => k.StreetName.ToUpper().Contains(searchColumnUlica.ToUpper()));
                    }

                }

            }

            return streetData;
        }

        public IEnumerable<Street> GetStreetData()
        {
            var street = DataSet.Include(p => p.Place);
            return street;
        }

        public int GetTotalStreetData()
        {
            var streetQuery = DataSet.AsQueryable();

            return streetQuery.Count();
        }
    }
}
