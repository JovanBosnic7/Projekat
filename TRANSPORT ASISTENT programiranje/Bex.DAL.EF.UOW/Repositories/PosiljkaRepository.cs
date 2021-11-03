using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class PosiljkaRepository : BaseRepository<Posiljka>, IPosiljkaRepository
    {
        public PosiljkaRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public PosiljkaRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        public IEnumerable<Posiljka> GetPosiljkaData()
        {
            var posiljka = DataSet.Include(k => k.PosiljkaKategorija)
            .Include(r => r.PosiljkaSadrzaj)
            .Include(s => s.PosiljkaStatus)
            .Include(f => f.PosiljkaVrsta)
            .Include(p => p.PosiljkaZadatak)
            ?.Include(u => u.PosiljkaUgovor)
            //?.Include(k => k.UserUneo)
            .Where(x=>x.Storno==false && x.Arhivirana==false);
            return posiljka;
        }

        public int GetTotalPosiljka()
        {
            var posiljkaQuery = DataSet.AsQueryable();

            return posiljkaQuery.Count();
        }

        public IEnumerable<Posiljka> GetSearchPosiljkaData(string searchTerms)

        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            int posiljkaId = 0;
            int ukupnoPaketa = 0;
            string status = "";
            string vrsta = "";
            string kategorija = "";
            string sadrzaj = "";
            string ugovor = "";
            int cenaUkupna = 0;
            string userDodao = "";
            string posiljalac = "";
            string posiljalacAdresa = "";
            string primalacAdresa = "";
            string primalac = "";
            string datum = "";

            var posiljka = GetPosiljkaData().AsQueryable();


            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];

                if (!String.IsNullOrEmpty(searchTxt)) {
                    if (searchColumn.Equals("PosiljkaId"))
                    {
                        posiljkaId = int.Parse(searchTxt);
                        posiljka = posiljka.Where(k => k.Id == posiljkaId);

                    }
                    
                    else if (searchColumn.Equals("UkupnoPaketa"))
                    {
                        ukupnoPaketa = int.Parse(searchTxt);
                        posiljka = posiljka.Where(k => k.UkupnoPaketa == ukupnoPaketa);
                    }
                    else if (searchColumn.Equals("Status"))
                    {

                        status = searchTxt;
                        posiljka = posiljka.Where(k => k.PosiljkaStatus.Naziv == status); //(searchColumnPravno.Equals("on") ? false : true)
                    }
                    else if (searchColumn.Equals("Vrsta"))
                    {
                        vrsta = searchTxt;
                        posiljka = posiljka.Where(k => k.PosiljkaVrsta.NazivVrste == vrsta);
                    }
                    else if (searchColumn.Equals("Kategorija"))
                    {
                        kategorija = searchTxt;
                        posiljka = posiljka.Where(k => k.PosiljkaKategorija.NazivKategorije == kategorija);
                    }
                    else if (searchColumn.Equals("Sadrzaj"))
                    {
                        sadrzaj = searchTxt;
                        posiljka = posiljka.Where(k => k.PosiljkaSadrzaj.Naziv == sadrzaj);
                    }
                    else if (searchColumn.Equals("Ugovor"))
                    {
                        ugovor = searchTxt;
                        posiljka = posiljka.Where(k => k.PosiljkaUgovor.Kontakt.Naziv.ToUpper().Contains(ugovor.ToUpper()));
                    }
                    else if (searchColumn.Equals("CenaUkupna"))
                    {
                        cenaUkupna = int.Parse(searchTxt);
                        posiljka = posiljka.Where(k => k.CenaUkupna == cenaUkupna);
                    }
                    else if (searchColumn.Equals("UserDodao"))
                    {
                        userDodao = searchTxt;
                        //posiljka = posiljka.Where(k => k.UserUneo.Kontakt.Naziv.ToUpper().Contains(userDodao.ToUpper()));
                    }
                    else if (searchColumn.Equals("DatumEvidentiranja"))
                    {
                        //datum = searchTxt;
                        string[] arrDatum = searchTxt.Split('/');
                        DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                        posiljka = posiljka.Where(k => k.DatumEvidentiranja == thisDate1);
                    }
                    else if (searchColumn.Equals("Posiljalac"))
                    {
                        posiljalac = searchTxt;
                        posiljka = posiljka.Where(k => k.PosiljkaZadatak.Where(x => x.Tip == "P").FirstOrDefault().NazivKlijenta.ToUpper().Contains(posiljalac.ToUpper()));
                    }
                    else if (searchColumn.Equals("Primalac"))
                    {
                        primalac = searchTxt;
                        posiljka = posiljka.Where(k => k.PosiljkaZadatak.Where(z => z.Tip == "D").FirstOrDefault().NazivKlijenta.ToUpper().Contains(primalac.ToUpper()));
                    }
                    else if (searchColumn.Equals("PosiljalacAdresa"))
                    {
                        posiljalacAdresa = searchTxt;
                        posiljka = posiljka.Where(x => x.PosiljkaZadatak.Where(p => p.Tip == "P").FirstOrDefault().Adresa.ToUpper().Contains(posiljalacAdresa.ToUpper()));

                    }
                    else if (searchColumn.Equals("PrimalacAdresa"))
                    {
                        primalacAdresa = searchTxt;
                        posiljka = posiljka.Where(x => x.PosiljkaZadatak.Where(p => p.Tip == "D").FirstOrDefault().Adresa.ToUpper().Contains(primalacAdresa.ToUpper()));

                    }
                }

            }

            return posiljka;
        }

        private IUowCommandResultFactory UowCommandResultFactory { get; }
    }
}
