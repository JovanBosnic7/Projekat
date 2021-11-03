using System.Collections.Generic;
using Bex.Models;
using System;

namespace BexMVC.ViewModels
{
   public class AlarmIndexData
   {
        //public IEnumerable<VozniParkAlarm> VozniParkAlarm { get; set; }
        //public IEnumerable<VozniParkDnevnik> VozniParkDnevnik { get; set; }
        public int Id { get; set; }
        public int? VpId { get; set; }
        public string Vozilo { get; set; }
        public string Registracija { get; set; }
        public string Alarm { get; set; }
        public int VpAlarmTip { get; set; }
        //public virtual VozniParkDnevnikTip VozniParkDnevnikTip { get; set; }
        public DateTime? Datum { get; set; }
        public int? Km { get; set; }
        public DateTime? DatumIsteka { get; set; }
        public int KmIsteka { get; set; }
        public DateTime? DatumAlarma { get; set; }
        public int KmAlarma { get; set; }
        public string Napomena { get; set; }
        public int Kolicina { get; set; }
        public int Cena { get; set; }
        public int IznosDin { get; set; }
        public decimal IznosEur { get; set; }
        public string Opis { get; set; }

        public int VpDnevnikId { get; set; }

    }
}