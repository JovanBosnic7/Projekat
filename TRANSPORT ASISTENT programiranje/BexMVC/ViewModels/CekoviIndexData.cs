using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class CekoviIndexData
   {
        public int Id { get; set; }
        public DateTime DatumDospeca { get; set; }
       
        public string BrojCeka { get; set; }
        public int Iznos { get; set; }

        public string TekuciRacun { get; set; }

        public int BankaId { get; set; }
        public string BankaNaziv { get; set; }
        public string BankaTekuciRacun { get; set; }

        public DateTime? DatumUnosa { get; set; }
        public string UserUneo { get; set; }
        public int BrojSpecifikacije { get; set; }
        public bool InternoRazduzen { get; set; }

        public DateTime? DatumRealizacije { get; set; }
        public string ZastupnikNaziv { get; set; }
        public int ProvizijaIznos { get; set; }
        public int Realizovano { get; set; }
        public int BrojCekova { get; set; }
        public bool RealizovanCek { get; set; }

        public string Napomena { get; set; }
        public bool Storno { get; set; }

    }
}