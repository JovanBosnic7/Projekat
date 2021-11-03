using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(KorisniciProgramaMetadata))]
    public partial class KorisniciPrograma
    {
        public class KorisniciProgramaMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Kontakt")]
            public int? KontaktId { get; set; }
            public string KorisnickoIme { get; set; }
            //public string Lozinka { get; set; }
            public string BarKod { get; set; }
            [ForeignKey("Region")]
            public int? RegionId { get; set; }

            //public int? UserId { get; set; }
            public bool Klijent { get; set; }
            public bool Aktivan { get; set; }

            public string AspNetUserId { get; set; }
            public string RoleId { get; set; }

           public int? idStari { get; set; }
            public int? FirmaId { get; set; }

            public object Kontakt { get; set; }
           
            public object Region { get; set; }
            private KorisniciProgramaMetadata() { }
        }
    }
}
