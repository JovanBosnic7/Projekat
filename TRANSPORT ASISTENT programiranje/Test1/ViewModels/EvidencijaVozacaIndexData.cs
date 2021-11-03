using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace DDtrafic.ViewModels
{
   public class EvidencijaVozacaIndexData
    {
        public int Id { get; set; }
        public string Vozac { get; set; }
        public string Dan { get; set; }
        public DateTime? Datum { get; set; }
        public string VremePocetka { get; set; }
        public string VremeZavrsetka { get; set; }
        public string RadniDan { get; set; }
        public string DnevnoRadnoVreme { get; set; }
        public string UpravljanjeVozilom { get; set; }
        public string OstaloRadnoVreme { get; set; }
        public string VremeRaspolozivosti { get; set; }
        public string VremeOdmora { get; set; }
        public string PlacenoOdsustvo { get; set; }
        public string NocniRad { get; set; }

    }
}