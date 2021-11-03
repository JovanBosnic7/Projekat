using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class KontaktRepository : BaseRepository<Kontakt>, IKontaktRepository
    {
        public KontaktRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public KontaktRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        public IEnumerable<Kontakt> GetKontaktData()
        {
            var kontakt = DataSet;
            //    .Include(k => k.Fizicko)
            //.Include(r => r.Pravno)
            //.Include(s => s.Telefon)
            //.Include(f => f.KontaktAdresa)
            //.Include(m => m.Email)
            //.Include(z => z.ZiroRacun)
            //.Include(o => o.Delatnost);
            //.Include(t => t.Zaposleni)
            //.Include(g => g.User);
            return kontakt;
        }

        
        public int GetTotalKontakts()
        {
            var kontaktsQuery = DataSet.AsQueryable();

            return kontaktsQuery.Count();
        }

        public IEnumerable<Kontakt> GetSearchKontaktDataRepository(string searchTerms)
        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnNaziv = "";
            string searchColumnAdresa = "";
            bool searchColumnPravno = false;
            bool searchColumnStranac = false;

            var kontakt = DataSet.AsQueryable();



            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt))
                {
                    if (searchColumn.Equals("KontaktNaziv"))
                    {
                        searchColumnNaziv = searchTxt;
                        kontakt = DataSet.AsQueryable().Where(k => k.Naziv.ToUpper().Contains(searchColumnNaziv.ToUpper()));

                    }
                    else if (searchColumn.Equals("Adresa"))
                    {
                        searchColumnAdresa = searchTxt;
                        kontakt = kontakt.Where(k => k.AdresaTekst.ToUpper().Contains(searchColumnAdresa.ToUpper()));
                    }
                    else if (searchColumn.Equals("Pravno") && !searchTxt.Equals("0"))
                    {
                        searchColumnPravno = searchTxt.Equals("1") ? true : false;
                        kontakt = kontakt.Where(k => k.PravnoLice == searchColumnPravno);
                    }
                    else if (searchColumn.Equals("Stranac") && !searchTxt.Equals("0"))
                    {
                        searchColumnStranac = searchTxt.Equals("1") ? true : false; ;
                        kontakt = kontakt.Where(k => k.Stranac == searchColumnStranac);
                    }
                }


            }

            return kontakt;
        }

        public IEnumerable<Kontakt> GetKontaktAutocompliteData(string searchQuery, bool isPravno, bool isAll)
        {
            var kontaktData = DataSet.AsQueryable();

            if (isAll)
            {
                kontaktData = kontaktData.Where(x => (x.Naziv.ToUpper() + " - " + x.AdresaTekst.ToUpper()).Contains(searchQuery.ToUpper()));
                
            }
            else
            {
                kontaktData = kontaktData.Where(a => a.PravnoLice == isPravno)
                                       .Where(x => (x.Naziv.ToUpper() + " - " + x.AdresaTekst.ToUpper()).Contains(searchQuery.ToUpper()));
            }

            

            return kontaktData;
        }

        private IUowCommandResultFactory UowCommandResultFactory { get; }
    }
}
