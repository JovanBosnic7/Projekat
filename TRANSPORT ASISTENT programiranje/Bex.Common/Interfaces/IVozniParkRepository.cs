using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IVozniParkRepository : IRepository<VozniPark>
    {
        IEnumerable<VozniPark> GetVozniParkData();

        IEnumerable<VozniPark> GetVozniParkNaziviData();
        IUowCommandResult UbaciUzonu(int id);
        IUowCommandResult DodajAlarm(int id, int vpAlarmTip = 0, DateTime? datum = null, int kilometraza = 0, DateTime? datumIsteka = null, int kmIsteka = 0, DateTime? datumAlarma = null, string napomena = "", int kolicina = 0, int cena = 0, int iznosDin = 0, decimal iznosEur = 0, string opis = "");
        IUowCommandResult DodajIzmeniStetu(int id, int? firmaId = 0, int? voziloId = 0, int? userUnosaId = 0, int? tipStete = 0, string opis = "", string napomena = "", int? stetocinaZaposleniId = -1, int? stetocinaCentarId = -1, DateTime? datumPredajePS = null, int? iznosDin = 0,
            int? iznosEur = 0, int? iznosZaNaplatu = 0, int? userId = 0, bool? sporno = false, bool? racun = false, bool? kes = false, DateTime? datumOdluke = null, bool? knjigovodstveniManjak = false, int? valutaId = 0, bool? potpisanaOdluka = false, bool? nenaplativo = false);
        IEnumerable<VozniPark> GetSearchVozniParkData(string searchTerms);
        int GetTotalVozniPark();

    }
}
