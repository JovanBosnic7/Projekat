using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class VozniParkRepository : BaseRepository<VozniPark>, IVozniParkRepository
    {
        public VozniParkRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public VozniParkRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }



        public IEnumerable<VozniPark> GetVozniParkData()
        {
            var vozniPark = DataSet.Include(g => g.Gorivo)
            .Include(o => o.VozniParkKaroserija)
            .Include(n => n.VozniParkMenjac)
            .Include(j => j.VozniParkKategorija)
            .Include(l => l.VozniParkModel)
            .Include(l => l.VozniParkModel.VozniParkMarka)
            .Include(h => h.VozniParkStatus);
            return vozniPark;
        }

        public IEnumerable<VozniPark> GetVozniParkNaziviData()
        {
            var vozniPark = DataSet;
            return vozniPark;
        }

        public int GetTotalVozniPark()
        {
            var vozniParkQuery = DataSet.AsQueryable();

            return vozniParkQuery.Count();
        }

        public IEnumerable<VozniPark> GetSearchVozniParkData(string searchTerms)

        {

            string[] terms = searchTerms.Split(',');
            string searchColumn = "";
            string searchTxt = "";

            string searchColumnModel = "";
            string searchColumnGarazniBroj = "";
            string searchColumnRegistracija = "";
            string searchColumnRegion = "";
            string searchColumnStatus = "";
            string searchColumnMarka = "";
            string searchColumnOpis = "";
            string searchColumnSasija = "";
            int searchColumnGodiste = 0;
            string searchColumnNapomena = "";
            string datum = "";
            int searchColumnBrojZone = 0;
            string searchColumnFirmaOS = "";
            string searchColumnNamena = "";
            string searchColumnBarkod = "";
            decimal searchColumnPropisanaPotrosnja = 0;
            string searchColumnSektor = "";


            var vozniPark = GetVozniParkData().AsQueryable();



            foreach (string t in terms)
            {
                string[] searchCT = t.Split(':');
                searchColumn = searchCT[0];
                searchTxt = searchCT[1];


                if (searchColumn.Equals("Model") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnModel = searchTxt;
                    vozniPark = DataSet.AsQueryable().Where(k => k.VozniParkModel.NazivModela.ToString().ToUpper().Contains(searchColumnModel.ToUpper()));

                }
                else if (searchColumn.Equals("Marka") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnMarka = searchTxt;
                    vozniPark = vozniPark.Where(k => k.VozniParkModel.VozniParkMarka.NazivMarke.ToUpper().Contains(searchColumnMarka.ToUpper()));
                }
                else if (searchColumn.Equals("Registracija") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnRegistracija = searchTxt;
                    vozniPark = vozniPark.Where(k => k.Oznaka.ToUpper().Contains(searchColumnRegistracija.ToUpper()));
                }
                else if (searchColumn.Equals("GarazniBroj") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnGarazniBroj = searchTxt;
                    vozniPark = vozniPark.Where(k => k.GarazniBroj.ToUpper().Contains(searchColumnGarazniBroj.ToUpper()));
                }
                else if (searchColumn.Equals("Statusi") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnStatus = searchTxt;
                    vozniPark = vozniPark.Where(k => k.VozniParkStatus.NazivStatusa.ToUpper().Contains(searchColumnStatus.ToUpper()));
                }
                else if (searchColumn.Equals("Marka") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnMarka = searchTxt;
                    vozniPark = vozniPark.Where(k => k.VozniParkModel.VozniParkMarka.NazivMarke.ToUpper().Contains(searchColumnMarka.ToUpper()));
                }
                else if (searchColumn.Equals("Sasija") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnSasija = searchTxt;
                    vozniPark = vozniPark.Where(k => k.Sasija.ToUpper().Contains(searchColumnSasija.ToUpper()));
                }
                else if (searchColumn.Equals("Godiste") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnGodiste = System.Convert.ToInt32(searchTxt);
                    vozniPark = vozniPark.Where(k => k.GodinaProizvodnje.Equals(searchColumnGodiste));
                }
                else if (searchColumn.Equals("Napomena") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnNapomena = searchTxt;
                    vozniPark = vozniPark.Where(k => k.Napomena.ToUpper().Contains(searchColumnNapomena.ToUpper()));
                }
                else if (searchColumn.Equals("DatumRegistracije") && !String.IsNullOrEmpty(searchTxt))
                {
                    //searchColumnDatumRegistracije = searchTxt;
                    string[] arrDatum = searchTxt.Split('/');
                    DateTime thisDate1 = new DateTime(int.Parse(arrDatum[2]), int.Parse(arrDatum[0]), int.Parse(arrDatum[1]));
                    vozniPark = vozniPark.Where(k => k.DatumRegistracije.Equals(thisDate1));
                }
                else if (searchColumn.Equals("PropisanaPotrosnja") && !String.IsNullOrEmpty(searchTxt))
                {
                    searchColumnPropisanaPotrosnja = System.Convert.ToDecimal(searchTxt); ;
                    vozniPark = vozniPark.Where(k => k.PropisanaPotrosnja.Equals(searchColumnPropisanaPotrosnja));
                }

            }
            return vozniPark;
        }

        public IUowCommandResult UbaciUzonu(int id)
        {
            var sqlString = @"EXEC prVozniParkDodajZonu " + id.ToString();

            return UowCommandResultFactory.Invoke(() =>
            {                
                return DataContext.Database.ExecuteSqlCommand(sqlString, id);
            });
        }

        public IUowCommandResult DodajAlarm(int id, int vpAlarmTip = 0, DateTime? datum = null, int kilometraza = 0, DateTime? datumIsteka = null, int kmIsteka = 0, DateTime? datumAlarma = null, string napomena = "", int kolicina=0, int cena=0, int iznosDin=0,decimal iznosEur=0, string opis="")
        {
            var sqlString = @"EXEC prVozniParkDodajAlarm " + id.ToString() + "," + vpAlarmTip.ToString() + "," + Datumtoformatzabazu(datum) + "," + kilometraza.ToString() + "," + Datumtoformatzabazu(datumIsteka) + "," + kmIsteka.ToString() + "," + Datumtoformatzabazu(datumAlarma) + "," + napomena + "," + kolicina.ToString() + "," + cena.ToString() + "," + iznosDin.ToString() + "," + iznosEur.ToString() + "," + opis;

            return UowCommandResultFactory.Invoke(() =>
            {
                return DataContext.Database.ExecuteSqlCommand(sqlString, id);
            });
        }

        public IUowCommandResult DodajIzmeniStetu(int id = 0, int? firmaId = 0, int? voziloId = 0, int? userUnosaId = 0, int? tipStete = 0, string opis = "", string napomena = "", int? stetocinaZaposleniId = 0, int? stetocinaCentarId = 0, DateTime? datumPredajePS = null, int? iznosDin = 0,
            int? iznosEur = 0, int? iznosZaNaplatu = 0, int? userId = 0, bool? sporno = false, bool? racun = false, bool? kes = false, DateTime? datumOdluke = null, bool? knjigovodstveniManjak = false, int? valutaId = 0, bool? potpisanaOdluka = false, bool? nenaplativo = false)
        {
            if (stetocinaCentarId > 0)
            {
                stetocinaZaposleniId = -1;
            }

            var sqlString = @"EXEC [prStetaVozniPark] " + id.ToString() + "," + firmaId.ToString() + "," + (voziloId ?? 0).ToString() + "," + (userUnosaId ?? 0).ToString() + "," + tipStete.ToString() + ",'" + opis + "','"
                 + napomena + "'," + (stetocinaZaposleniId ?? -1).ToString() + "," + (stetocinaCentarId ?? -1).ToString() + "," + Datumtoformatzabazu(datumPredajePS) + "," + (iznosDin ?? 0).ToString() + "," + iznosEur.ToString() + ","
                 + (iznosZaNaplatu ?? 0).ToString() + "," + (userId ?? 0).ToString() + "," + Convert.ToInt16(sporno) + "," + Convert.ToInt16(racun) + "," + Convert.ToInt16(kes) + "," + Datumtoformatzabazu(datumOdluke) + "," + Convert.ToInt16(knjigovodstveniManjak) + ","
                 + valutaId.ToString() + "," + Convert.ToInt16(potpisanaOdluka) + "," + Convert.ToInt16(nenaplativo);

            return UowCommandResultFactory.Invoke(() =>
            {
                return DataContext.Database.ExecuteSqlCommand(sqlString, voziloId);
            });
        }

        private static string Datumtoformatzabazu(DateTime? datum)
        {
            string dat="NULL";
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
