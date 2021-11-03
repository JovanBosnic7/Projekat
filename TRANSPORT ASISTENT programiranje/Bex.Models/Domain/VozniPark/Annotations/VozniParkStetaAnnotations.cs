using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkStetaMetadata))]
    public partial class VozniParkSteta
    {
        public class VozniParkStetaMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Firma")]
            public int? FirmaSteteId { get; set; }
            [ForeignKey("VozniPark")]
            public int? ZavisnaTabelaId { get; set; }
            [ForeignKey("KorisnikIzdao")]
            public int? NalogIzdaoID { get; set; }
            [ForeignKey("StetaKategorija")]
            public int? NalogSektorId { get; set; }
            [ForeignKey("StetaTip")]
            public int? KategorijaId { get; set; }
            public string Opis { get; set; }
            public string Napomena { get; set; }
            [ForeignKey("StetocinaZaposleni")]
            public int? StetocinaZaposleniId { get; set; }
            [ForeignKey("StetocinaCentar")]
            public int? StetocinaCentarId { get; set; }
            public DateTime? DatumPredajePravnoj { get; set; }
            public int? IznosRsd { get; set; }
            public int? IznosZaNaplatu { get; set; }
            public DateTime? DatumUnosa { get; set; }
            [ForeignKey("KorisnikUneo")]
            public int? UserUnosId { get; set; }
            public bool? Sporno { get; set; }
            public bool? Racun { get; set; }
            public bool? Kes { get; set; }
            public bool? Storno { get; set; }
            public DateTime? DatumOdluke { get; set; }
            public bool? KnjigovodstveniManjak { get; set; }
            public int? ValutaId { get; set; }
            public bool? PotpisanaOdluka { get; set; }
            public bool? Nenaplativo { get; set; }

            public object Firma { get; set; }
            public object VozniPark { get; set; }           
            public object KorisnikIzdao { get; set; }
            public object StetaKategorija { get; set; }
            public object StetaTip { get; set; }
            public object StetocinaZaposleni { get; set; }
            public object StetocinaCentar { get; set; }
            public object KorisnikUneo { get; set; }

            private VozniParkStetaMetadata() { }
        }
    }
}
