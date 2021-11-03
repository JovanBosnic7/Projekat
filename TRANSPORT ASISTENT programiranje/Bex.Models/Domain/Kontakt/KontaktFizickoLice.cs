using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KontaktFizickoLice 
    {
        public KontaktFizickoLice()
        {

            
        }
        public int Id { get; set; }
        public int KontaktId { get; set; }

        public string MaticniBroj { get; set; }
        public DateTime? DatumRodjenja { get; set; }

        public string MestoRodjenja { get; set; }

        public string Drzavljanstvo { get; set; }

        public string  BrojLicneKarte { get; set; }

        public DateTime? DatumIstekaLicneKarte { get; set; }

        public string MestoIzdavanjaLicneKarte { get; set; }

        public string BrojVozackeDozvole { get; set; }

        public DateTime? DatumIstekaVozackeDozvole { get; set; }

        public string KategorijaVozackeDozvole { get; set; }

        public string MestoIzdavanjaVozackeDozvole { get; set; }

        public string BrojPasosa { get; set; }

        public DateTime? DatumIstekaPasosa { get; set; }

        public string MestoIzdavanjaPasosa { get; set; }

        public PolIzbor  Pol;

        public virtual Kontakt Kontakt { get; set; }
       
    }
}
