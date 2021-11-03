using System.Collections.Generic;
using Bex.Models;
using System;

namespace DDtrafic.ViewModels
{
   public class AlarmiIndexData
   {
        public int Id { get; set; }
        public int? VpId { get; set; }
        public int? ZaposleniId { get; set; }
        public string Registracija { get; set; }
        public string Zaposleni { get; set; }
        public string GrupaAlarma { get; set; }
        public string Alarm { get; set; }
        public int? VpAlarmTip { get; set; }
        public int? VpAlarmGrupaId { get; set; }
        public DateTime? Datum { get; set; }
        public int? Km { get; set; }
        public DateTime? DatumIsteka { get; set; }
        public int? KmIsteka { get; set; }
        public DateTime? DatumAlarma { get; set; }
        public int? KmAlarma { get; set; }
        public string Napomena { get; set; }
        public decimal? IznosDin { get; set; }
        public decimal? IznosEur { get; set; }

    }
}