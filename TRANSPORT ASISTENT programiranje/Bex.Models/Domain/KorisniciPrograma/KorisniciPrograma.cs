using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KorisniciPrograma
    {
       
        public int Id { get; set; }
        public int? KontaktId { get; set; }

        public string KorisnickoIme { get; set; }
        //public string Lozinka { get; set; }
        public string BarKod { get; set; }
        public int? RegionId { get; set; }

        //public int? UserId { get; set; }

        public bool Klijent { get; set; }
        public bool Aktivan { get; set; }

        public string AspNetUserId { get; set; }
        public string RoleId { get; set; }
        public int? idStari { get; set; }
        public int? FirmaId { get; set; }
        public virtual Kontakt Kontakt { get; set; }
        public virtual Region Region { get; set; }

        //public virtual ICollection<Posiljka> Posiljka { get; set; }
        public virtual ICollection<PosiljkaZadatak> PosiljkaZadatak { get; set; } //tetka zakomentarisala, ccko vratio. treba pogledati
        //public virtual ICollection<PosiljkaOtkupZbirni> PosiljkaOtkupZbirni { get; set; }
        //public virtual ICollection<PaketZadatak> PaketZadatak { get; set; }
        //public virtual ICollection<KurirZaduzenje> KurirZaduzenje { get; set; }
        public virtual ICollection<KurirRazduzenje> KurirRazduzenje { get; set; } //tetka zakomentarisala, ccko vratio. treba pogledati
        //public virtual ICollection<KurirRazduzenje> KurirRazduzenje { get; set; }
        public virtual ICollection<Trebovanje> Trebovanje { get; set; }
        public virtual ICollection<KurirRazduzenjeSpecifikacija> KurirRazduzenjeSpecifikacija { get; set; } //tetka zakomentarisala, ccko vratio. treba pogledati
        public virtual ICollection<NapomenaPosiljka> NapomenaPosiljka { get; set; } //tetka zakomentarisala, ccko otkomentarisao. treba proveriti

        //public virtual ICollection<WebFiles> WebFiles { get; set; }

        //public virtual ICollection<ZaposleniNapomena> ZaposleniNapomena { get; set; }

    }
}
