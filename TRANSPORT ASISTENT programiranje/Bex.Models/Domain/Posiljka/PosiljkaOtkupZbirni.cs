using AspNet.DAL.EF.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class PosiljkaOtkupZbirni
    {
 
        public int Id { get; set; }
        public string BarKod { get; set; }
        public int? ReonId { get; set; }
        public string NazivKlijenta { get; set; }
        public string Adresa { get; set; }
        public string Telefon { get; set; }
        public int? SubIdZaIzvestaje { get; set; }
        public string PoslednjaNapomena { get; set; }
        public decimal? UkupnoOtkupa { get; set; }
        public int? BrojOtkupa { get; set; }
        public int? IzvrsioId { get; set; }
        public DateTime? DatumIzvrsenja { get; set; }
        public TimeSpan? VremeIzvrsenja { get; set; }

        public DateTime Stamp { get; set; }

        public virtual ICollection<PosiljkaOtkupZbirniStavka> PosiljkaOtkupZbirniStavka { get; set; }
        public virtual KorisniciPrograma User { get; set; }
        public virtual Reon Reon { get; set; }


    }
}
