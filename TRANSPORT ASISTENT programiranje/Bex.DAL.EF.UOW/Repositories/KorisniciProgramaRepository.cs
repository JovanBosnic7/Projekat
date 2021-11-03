using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class KorisniciProgramaRepository : BaseRepository<KorisniciPrograma>, IKorisniciProgramaRepository
    {
        public KorisniciProgramaRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public KorisniciProgramaRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        
        

        public IEnumerable<KorisniciPrograma> GetUserData()
        {
            var users = DataSet.Include(k => k.Kontakt)
            .Include(r => r.Region);
           

            return users;
        }

        public int GetTotalUser()
        {
            var userQuery = DataSet.AsQueryable();

            return userQuery.Count();
        }

        public IEnumerable<KorisniciPrograma> GetSearchUserData(string searchTerms)

        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string korisnik = "";
            string korisnickoIme = "";
            string barkod = "";
            string region = "";
            int klijent = 0;
            int aktivan = 0;


            var user = GetUserData().AsQueryable();



            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("Korisnik") && !String.IsNullOrEmpty(searchTxt))
                {
                    korisnik = searchTxt;
                    user = DataSet.AsQueryable().Where(k => k.Kontakt.Naziv.ToString().ToUpper().Contains(korisnik.ToUpper()));

                }
                //else if (searchColumn.Equals("KorisnickoIme") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    korisnickoIme = searchTxt;
                //    user = user.Where(k => k.KorisnickoIme.ToUpper().Contains(korisnickoIme.ToUpper()));
                //}
                else if (searchColumn.Equals("BarKod") && !String.IsNullOrEmpty(searchTxt))
                {
                    barkod = searchTxt;
                    user = user.Where(k => k.BarKod.Contains(barkod.ToUpper()));
                }
                else if (searchColumn.Equals("Region") && !String.IsNullOrEmpty(searchTxt))
                {
                    region = searchTxt;
                    user = user.Where(k => k.Region.NazivSkraceni.ToUpper().Contains(region.ToUpper()));
                }
                else if (searchColumn.Equals("Klijent") && !String.IsNullOrEmpty(searchTxt))
                {
                    if (searchTxt == "da")
                    {
                        klijent = 1;
                    }

                    user = user.Where(k => k.Klijent == System.Convert.ToBoolean(aktivan));
                }
                else if (searchColumn.Equals("Aktivan") && !String.IsNullOrEmpty(searchTxt))
                {
                    if (searchTxt == "da")
                    {
                        aktivan = 1;                        
                    }
                    
                    user = user.Where(k => k.Aktivan== System.Convert.ToBoolean(aktivan));
                }


            }
            return user;
        }


        public IEnumerable<KorisniciPrograma> GetKorisniciProgramaAutocompliteData(string searchQuery)
        {
            var korisniciData = GetUserData().Where(x => (x.Kontakt.Naziv.ToUpper() + " - " + x.Id.ToString().ToUpper()).Contains(searchQuery.ToUpper()));

            return korisniciData;
        }


        private IUowCommandResultFactory UowCommandResultFactory { get; }
    }
}
