using AspNet.DAL.EF.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class Posiljka
    {
 
        public int Id { get; set; }
        public int? StatusId { get; set; }
        public DateTime DatumEvidentiranja { get; set; }
        public TimeSpan? VremeEvidentiranja { get; set; }
        public int? VrstaPosiljkeId { get; set; }
        public int? KategorijaPosiljkeId { get; set; }
        public int KategorijaIznos { get; set; }
        public int? SadrzajId { get; set; }
        public int CenaUkupna { get; set; }
        public int? UserDodaoId { get; set; }
        public int? NarucilacId { get; set; }
        public int UkupnoPaketa { get; set; }
        public int? ZakljucaoId { get; set; }
        public int? UgovorId { get; set; }
        public int? CenovnikId { get; set; }
        public bool? CenaPoPaketu { get; set; }
        public bool StornoZaKlijente { get; set; }
        public int? NacinUnosaId { get; set; }
        public bool Storno { get; set; }
        public bool Arhivirana { get; set; }

        public virtual PosiljkaStatus PosiljkaStatus { get; set; }
        public virtual PosiljkaVrsta PosiljkaVrsta { get; set; }
        public virtual PosiljkaKategorija PosiljkaKategorija { get; set; }
        public virtual PosiljkaSadrzaj PosiljkaSadrzaj { get; set; }

        public virtual Ugovor PosiljkaUgovor { get; set; }

        //public virtual KorisniciPrograma UserUneo { get; set; }


        public virtual ICollection<Paket> Paket { get; set; }
        public virtual ICollection<PosiljkaPlacanje> PosiljkaPlacanje { get; set; }
        public virtual ICollection<PosiljkaUsluga> PosiljkaUsluga { get; set; }
        public virtual ICollection<PosiljkaZadatak> PosiljkaZadatak { get; set; }
        public virtual ICollection<PosiljkaNapomena> PosiljkaNapomena { get; set; }

        public virtual ICollection<KurirRazduzenjeSpecifikacija> KurirRazduzenjeSpecifikacija { get; set; }
        public virtual ICollection<NapomenaPosiljka> NapomenaPosiljka { get; set; }

        public virtual ICollection<SkenRead> SkenRead { get; set; }



    }
}
