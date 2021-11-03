using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkDnevnikMetadata))]
    public partial class VozniParkDnevnik
    {

        public class VozniParkDnevnikMetadata
        {
            public int Id { get; set; }
            
            public DateTime? Datum { get; set; }
            [ForeignKey("VozniParkDnevnikTip")]
            public int DnevnikTipId { get; set; }
            [ForeignKey("Region")]
            public int RegionId { get; set; }
            [ForeignKey("VozniPark")]
            public int VpId { get; set; }
            public int? Km { get; set; }
            [ForeignKey("Artikli")]
            public int? ArtId { get; set; }
            [ForeignKey("MagacinSpisak")]
            public int? MagacinId { get; set; }
            public int? Kolicina { get; set; }
            public int? Cena { get; set; }
            public int? IznosDin { get; set; }
            public decimal? IznosEur { get; set; }
            public string Opis { get; set; }
            public string Napomena { get; set; }
            public int? UserUnosId { get; set; }
            public bool? NavOK { get; set; }

            public object VozniParkDnevnikTip { get; set; }
            public object VozniPark { get; set; }
            public object Region { get; set; }
            public object Artikli { get; set; }
            public object MagacinSpisak { get; set; }

            private VozniParkDnevnikMetadata() { }
        }
    }
}
