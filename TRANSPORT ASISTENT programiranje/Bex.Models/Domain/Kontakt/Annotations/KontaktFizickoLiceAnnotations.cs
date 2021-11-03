using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{

    [MetadataType(typeof(KontaktFizickoLiceMetadata))]
    public partial class KontaktFizickoLice
    {
        public class KontaktFizickoLiceMetadata 
        {
            
            public int Id { get; set; }
            [ForeignKey("Kontakt")]
            public int KontaktId { get; set; }

            public string MaticniBroj { get; set; }
            public DateTime DatumRodjenja { get; set; }

            //[MaxLength(10, ErrorMessage = "BloggerName must be 10 characters or less"), MinLength(5)]
            public string MestoRodjenja { get; set; }

            public string Drzavljanstvo { get; set; }

            public string BrojLicneKarte { get; set; }

            public DateTime DatumIstekaLicneKarte { get; set; }

            public string MestoIzdavanjaLicneKarte { get; set; }

            public string BrojVozackeDozvole { get; set; }

            public DateTime DatumIstekaVozackeDozvole { get; set; }

            public string KategorijaVozackeDozvole { get; set; }

            public string MestoIzdavanjaVozackeDozvole { get; set; }

            public string BrojPasosa { get; set; }

            public DateTime DatumIstekaPasosa { get; set; }

            public string MestoIzdavanjaPasosa { get; set; }

            //[StringLength(1, ErrorMessage = "Za muski pol M, a za zenski Z.")]
            //[EnumDataType(typeof(Pol), ErrorMessage = "Za muski pol M, a za zenski Z.")]
           // [Display(Name = "Your Property Name")]
           // public PolIzbor Pol { get; set; }

            public object Kontakt { get; set; }

            private KontaktFizickoLiceMetadata() { }

        }
    }
}
