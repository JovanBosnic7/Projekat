using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class CenovnikRepository : BaseRepository<Cenovnik>, ICenovnikRepository
    {
        public CenovnikRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public CenovnikRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }



        public IEnumerable<Cenovnik> GetCenovnikData()
        {
            var cenovnik = DataSet;//.Include(a => a.Cene);
                                   //.Include(r => r.Cene.)
                                   //.Include(s => s.Zaposleni.Kontakt)
                                   //.Include(f => f.KorisnikCenovnika);
                                   //.Include(m => m.Kontakt)
                                   //.Include(z => z.RegionId)
                                   //.Include(o => o.KorisniciPrograma.Kontakt);
                                   //.Include(t => t.PosrednikUserId)
                                   //.Include(n => n.UgovorBroj);
                                   //.Include(l => l.VozniParkModel)
                                   //.Include(h => h.VozniParkStatus)
                                   //.Include(v => v.VozniParkTip);
            return cenovnik;
        }





        public int GetTotalCenovnik()
        {
            var cenovnikQuery = DataSet.AsQueryable();

            return cenovnikQuery.Count();
        }

        public IEnumerable<Cenovnik> GetCenovnikAutocompleteData(string searchQuery)
        {
            var cenovnikData = DataSet.Where(x => (x.BrojCenovnika.ToString().ToUpper()).Contains(searchQuery.ToUpper()));

            return cenovnikData;
        }

        public IEnumerable<Cenovnik> GetSearchCenovnikData(string searchTerms)

        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnBrojCenovnika = "";
            //string searchColumnNosilac = "";
            //string searchColumnRegion = "";
            //string searchColumnUgovorio = "";
            //string searchColumnAgent = "";
            //string searchColumnVerzijaUgovora = "";
            //string searchColumnPosrednik = "";


            var cenovnik = DataSet.AsQueryable();



            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                //if (searchColumn.Equals("Model") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnModel = searchTxt;
                //    ugovor = DataSet.AsQueryable().Where(k => k..ToUpper().Contains(searchColumnModel.ToUpper()));

                //}
                //else if (searchColumn.Equals("Registracija") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnRegistracija = searchTxt;
                //    ugovor = vozniPark.Where(k => k.Oznaka.ToUpper().Contains(searchColumnRegistracija.ToUpper()));
                //}
                //else if (searchColumn.Equals("GarazniBroj") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnGarazniBroj = searchTxt;
                //    ugovor = vozniPark.Where(k => k.GarazniBroj.ToUpper().Contains(searchColumnGarazniBroj.ToUpper()));
                //}
                //else if (searchColumn.Equals("Region") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnRegion = searchTxt;
                //    ugovor = vozniPark.Where(k => k.Region.NazivSkraceni.ToUpper().Contains(searchColumnRegion.ToUpper()));
                //}
                //else if (searchColumn.Equals("Statusi") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnStatus = searchTxt;
                //    ugovor = vozniPark.Where(k => k.VozniParkStatus.NazivStatusa.ToUpper().Contains(searchColumnStatus.ToUpper()));
                //}

                if (searchColumn.Equals("BrojCenovnika") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnBrojCenovnika = searchTxt;
                    cenovnik = cenovnik.Where(k => k.BrojCenovnika.ToString().ToUpper().Contains(searchColumnBrojCenovnika.ToUpper()));
                }
                //else if (searchColumn.Equals("Nosilac") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnNosilac = searchTxt;
                //    ugovor = ugovor.Where(k => k.Kontakt.Naziv.ToUpper().Contains(searchColumnNosilac.ToUpper()));
                //}
                //else if (searchColumn.Equals("Region") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnRegion = searchTxt;
                //    ugovor = ugovor.Where(k => k.Region.NazivSkraceni.ToUpper().Contains(searchColumnRegion.ToUpper()));
                //}
                //else if (searchColumn.Equals("Ugovorio") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnUgovorio = searchTxt;
                //    ugovor = ugovor.Where(k => k.KorisniciPrograma.Kontakt.Naziv.ToUpper().Contains(searchColumnUgovorio.ToUpper()));
                //}
                //else if (searchColumn.Equals("Agent") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnAgent = searchTxt;
                //    ugovor = ugovor.Where(k => k.Zaposleni.Kontakt.Naziv.ToUpper().Contains(searchColumnAgent.ToUpper()));
                //}
                //else if (searchColumn.Equals("VerzijaUgovora") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnVerzijaUgovora = searchTxt;
                //    ugovor = ugovor.Where(k => k.UgovorVerzija.ToString().ToUpper().Contains(searchColumnVerzijaUgovora.ToUpper()));
                //}
                //else if (searchColumn.Equals("Posrednik") && !String.IsNullOrEmpty(searchTxt))
                //{
                //    searchColumnPosrednik = searchTxt;
                //    ugovor = ugovor.Where(k => k.KorisniciPrograma.Kontakt.Naziv.ToUpper().Contains(searchColumnPosrednik.ToUpper()));
                //}
                //else if (searchColumn.Equals("DatumUgovora"))
                //{
                //    //datum = searchTxt;
                //    string[] arrDatum = searchTxt.Split('/');
                //    DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                //    ugovor = ugovor.Where(k => k.DatumUgovora == thisDate1);
                //}
                //else if (searchColumn.Equals("DatumVerzije"))
                //{
                //    //datum = searchTxt;
                //    string[] arrDatum2 = searchTxt.Split('/');
                //    DateTime thisDate2 = new DateTime(int.Parse(arrDatum2[2]), int.Parse(arrDatum2[0]), int.Parse(arrDatum2[1]));
                //    ugovor = ugovor.Where(k => k.DatumPoslednjeVerzije == thisDate2);
                //}
                //else if (searchColumn.Equals("DatumObnove"))
                //{
                //    //datum = searchTxt;
                //    string[] arrDatum3 = searchTxt.Split('/');
                //    DateTime thisDate3 = new DateTime(int.Parse(arrDatum3[2]), int.Parse(arrDatum3[0]), int.Parse(arrDatum3[1]));
                //    ugovor = ugovor.Where(k => k.DatumObnove == thisDate3);
                //}
            }
            return cenovnik;
        }

        //public IUowCommandResult UbaciUzonu(int id)
        //{
        //    var sqlString = @"EXEC prVozniParkDodajZonu " + id.ToString();

        //    return UowCommandResultFactory.Invoke(() =>
        //    {                
        //        return DataContext.Database.ExecuteSqlCommand(sqlString, id);
        //    });
        //}

        //public IUowCommandResult DodajAlarm(int id, int vpAlarmTip = 0, DateTime? datum = null, int kilometraza = 0, DateTime? datumIsteka = null, int kmIsteka = 0, DateTime? datumAlarma = null, string napomena = "", int kolicina=0, int cena=0, int iznosDin=0,decimal iznosEur=0, string opis="")
        //{
        //    var sqlString = @"EXEC prVozniParkDodajAlarm " + id.ToString() + "," + vpAlarmTip.ToString() + "," + Datumtoformatzabazu(datum) + "," + kilometraza.ToString() + "," + Datumtoformatzabazu(datumIsteka) + "," + kmIsteka.ToString() + "," + Datumtoformatzabazu(datumAlarma) + "," + napomena + "," + kolicina.ToString() + "," + cena.ToString() + "," + iznosDin.ToString() + "," + iznosEur.ToString() + "," + opis;

        //    return UowCommandResultFactory.Invoke(() =>
        //    {
        //        return DataContext.Database.ExecuteSqlCommand(sqlString, id);
        //    });
        //}

        private static string Datumtoformatzabazu(DateTime? datum)
        {
            string dat = "NULL";
            if (datum.HasValue == true)
            {
                dat = datum.Value.Year + @"-" + Stringdopunisleva(datum.Value.Month.ToString(), 2, "0") + @"-" + Stringdopunisleva(datum.Value.Day.ToString(), 2, "0");
            }
            return "'" + dat + "'";
        }

        private static string Stringdopunisleva(string str, int ukupnoznakova, string znakzadopunjavanje)
        {
            if (str.Length < ukupnoznakova)
            {
                str = Stringistiznakovi(ukupnoznakova - str.Length, znakzadopunjavanje) + str;
            }
            return str;
        }

        private static string Stringistiznakovi(int brojznakova, string znak)
        {
            string x = "";
            if (znak.Length <= 0) znak = " ";
            while (x.Length < brojznakova) x += znak;
            return x;
        }

        private IUowCommandResultFactory UowCommandResultFactory { get; }
    }
}
