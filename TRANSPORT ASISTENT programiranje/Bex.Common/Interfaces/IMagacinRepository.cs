using Bex.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.Common
{
    public interface IMagacinRepository : IRepository<KurirZaduzenje>
    {
        IEnumerable<KurirZaduzenje> GetZaduzenjaKurira();
        //IUowCommandResult UbaciUzonu(int id);
        //IUowCommandResult DodajAlarm(int id, int vpAlarmTip = 0, DateTime? datum = null, int kilometraza = 0, DateTime? datumIsteka = null, int kmIsteka = 0, DateTime? datumAlarma = null, string napomena = "", int kolicina = 0, int cena = 0, int iznosDin = 0, decimal iznosEur = 0, string opis = "");
        //IEnumerable<VozniPark> GetSearchVozniParkData(string searchTerms);
        int GetTotalZaduzenja();

        IUowCommandResult KreirajZbirneOtkupe(List<string> listaRegiona);

        // IEnumerable<ZaduzenjeKurira> GetSearchZaduzenjaKuriraData(string searchTerms, List<ZaduzenjeKurira> searchDataSet);

    }
}
