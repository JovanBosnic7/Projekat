using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PrijavaReklamacijaZalbaLogMetadata))]
    public partial class PrijavaReklamacijaZalbaLog
    {
        public class PrijavaReklamacijaZalbaLogMetadata
        {

            public int? Id { get; set; }
            public int? PrijavaId { get; set; }

            [ForeignKey("PrijavaTip")]
            public int? TipPrijaveId { get; set; }
            [ForeignKey("PrijavaPodTip")]
            public int? PodTipPrijaveId { get; set; }
            [ForeignKey("PrijavaNacin")]
            public int? NacinPrijaveId { get; set; }
            //[ForeignKey("Posiljka2")]
            public int? PosiljkaId { get; set; }
            [ForeignKey("KorisniciPrograma")]
            public int? PrijavioUserId { get; set; }
            public string PrijavioIme { get; set; }
            public string PrijavioPrezime { get; set; }
            public string PrijavioEmail { get; set; }
            public string PrijavioTelefon { get; set; }
            public string Opis { get; set; }
            [ForeignKey("PrijavaStatus")]
            public int? StatusPrijaveId { get; set; }
            public DateTime? DatumPrijave { get; set; }
            public DateTime? DatumIzmene { get; set; }
            public int? UserIdIzmene { get; set; }

            public object PrijavaTip { get; set; }
            public object PrijavaPodTip { get; set; }
            public object PrijavaNacin { get; set; }
            //public object Posiljka2 { get; set; }
            public object KorisniciPrograma { get; set; }
            public object PrijavaStatus { get; set; }
            private PrijavaReklamacijaZalbaLogMetadata() { }
        }
    }
}