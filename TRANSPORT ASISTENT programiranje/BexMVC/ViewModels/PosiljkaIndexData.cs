using System.Collections.Generic;
using Bex.Models;
using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace BexMVC.ViewModels
{
   public class PosiljkaIndexData
    {
        
        public int PosiljkaId { get; set; }

        public int UkupnoPaketa { get; set; }

        public string Status { get; set; }

        public IEnumerable<SelectListItem> StatusiCmb { get; set; }

        public string Vrsta { get; set; }

        public string Kategorija { get; set; }

        public string Sadrzaj { get; set; }

        public string Ugovor { get; set; }

        public int CenaUkupna { get; set; }

        public string UserDodao { get; set; }
        public bool Storno { get; set; }
        public bool Arhivirana { get; set; }


        public DateTime DatumEvidentiranja { get; set; }
        public TimeSpan? VremeEvidentiranja { get; set; }

        public string Posiljalac { get; set; }
        public string PosiljalacAdresa { get; set; }
        public string Primalac { get; set; }
        public string PrimalacAdresa { get; set; }



        //public IEnumerable<Kontakt> Kontakts { get; set; }
        //public KontaktFizickoLice KontaktsFizickaLica { get; set; }

        //public KontaktPravnoLice KontaktsPravnaLica { get; set; }
        //public IEnumerable<KontaktAdresa> KontaktAdresa { get; set; }
        //public IEnumerable<KontaktEmail> KontaktEmail { get; set; }
        //public IEnumerable<KontaktTelefon> KontaktTelefon { get; set; }
        //public IEnumerable<KontaktZiroRacun> KontaktZiroRacun { get; set; }
        //public IEnumerable<KontaktDelatnost> KontaktDelatnost { get; set; }

        //public Kontakt Kontakti { get; set; }
    }
}