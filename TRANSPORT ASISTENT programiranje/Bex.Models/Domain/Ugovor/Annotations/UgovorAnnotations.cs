using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(UgovorMetadata))]
    public partial class Ugovor
    {
        public class UgovorMetadata
        {

            public int? Id { get; set; }
            [ForeignKey("Kontakt")]
            public int? KontaktIdNosilac { get; set; }
            [ForeignKey("Cenovnik")]
            public int? Cenovnik1Id { get; set; }
            public int? Cenovnik2Id { get; set; }
            public int? UgovorBroj { get; set; }
            public int? UgovorVerzija { get; set; }
            public DateTime? DatumPoslednjeVerzije { get; set; }
            public DateTime? DatumUgovora { get; set; }
            public DateTime? DatumObnove { get; set; }
            public DateTime? DatumIOSa { get; set; }
            [ForeignKey("Region")]
            public int? RegionId { get; set; }
            public string ZastupnikKlijenta { get; set; }
            [ForeignKey("KorisniciPrograma")]
            public int? UgovorilacUserId { get; set; }
            [ForeignKey("Zaposleni")]
            public int? AgentZaposleniId { get; set; }
            //[ForeignKey("KorisniciPrograma")]
            public int? UserIdDodao { get; set; }
            //[ForeignKey("KorisniciPrograma")]
            public int? PosrednikUserId { get; set; }
            public int? KompanijaId { get; set; }
            public bool VracenAneks { get; set; }
            public bool NeRacunaSeAnaliza2 { get; set; }
            public bool SezonskiKlijent { get; set; }
            public string SezonaSlanja { get; set; }
            public int? ObecanoPaketa { get; set; }
            public int? ObecanPromet { get; set; }
            public int? StariBrojUgovora { get; set; }
            public bool Fiktivan { get; set; }
            public bool Aktivan { get; set; }
            public DateTime? DatumUnosa { get; set; }
            [NotMapped]
            public string NosilacNaziv { get; set; }
            [NotMapped]
            public string ZaposleniNaziv { get; set; }
            [NotMapped]
            public string PosrednikNaziv { get; set; }
            [NotMapped]
            public string UgovorilacNaziv { get; set; }
            [NotMapped]
            public int Cenovnik1Broj { get; set; }
            [NotMapped]
            public int Cenovnik2Broj { get; set; }
            public object Kontakt { get; set; }
            public object Region { get; set; }
            public object Zaposleni { get; set; }
            public object KorisniciPrograma { get; set; }
            public object Cenovnik { get; set; }

            private UgovorMetadata() { }
        }
    }
}