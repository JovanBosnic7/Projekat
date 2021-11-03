using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class VozniPark
    {
 
        public int Id { get; set; }
        public string GarazniBroj { get; set; }
        public string Oznaka { get; set; }
        public string Napomena { get; set; }
        public DateTime? DatumRegistracije { get; set; }
        public int? ModelId { get; set; }     
        public string Sasija { get; set; }
        public string Motor { get; set; }
        public int? MenjacId { get; set; }
        public decimal? PropisanaPotrosnja { get; set; }
        public int? GodinaProizvodnje { get; set; }
        public int? Nosivost { get; set; }
        public int? Masa { get; set; }
        public int? BrojVrata { get; set; }
        public decimal? Snaga { get; set; }
        public int? Zapremina { get; set; }
        public string TipVozila { get; set; }
        public int? KmNabavna { get; set; }
        public int? GorivoId { get; set; }
        public int? KaroserijaId { get; set; }
        public int? PodKaroserijaId { get; set; }
        public int? NDM { get; set; }
        public int? StatusVozilaId { get; set; }
        public int? KategorijaId { get; set; }
        public int? BrMestaSedenje { get; set; }
        public int? BrMestaStajanje { get; set; }
        public string PogonskiPneumatici { get; set; }
        public string UpravljackiPneumatici { get; set; }
        public int? FirmaId { get; set; }
        public bool Aktivno { get; set; }
        public string Sifra { get; set; }

        public virtual Gorivo Gorivo { get; set; }
        public virtual VozniParkKaroserija VozniParkKaroserija { get; set; }
        public virtual VozniParkPodKaroserija VozniParkPodKaroserija { get; set; }
        public virtual VozniParkMenjac VozniParkMenjac { get; set; }
        public virtual VozniParkKategorija VozniParkKategorija { get; set; }
        public virtual VozniParkModel VozniParkModel { get; set; }
        public virtual VozniParkStatus VozniParkStatus { get; set; }
       

    }
}
