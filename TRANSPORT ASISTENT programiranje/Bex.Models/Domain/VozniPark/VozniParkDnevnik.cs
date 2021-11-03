using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class VozniParkDnevnik
    {
        public VozniParkDnevnik()
        {


        }
        public int Id { get; set; }
        public DateTime? Datum { get; set; }
        public int DnevnikTipId { get; set; }
        public int? RegionId { get; set; }
        public int? VpId { get; set; }
        public int? Km { get; set; }
        public int? ArtId { get; set; }
        public int? MagacinId { get; set; }
        public int Kolicina { get; set; }
        public int Cena { get; set; }
        public int IznosDin { get; set; }
        public decimal IznosEur { get; set; }
        public string Opis { get; set; }
        public string Napomena { get; set; }
        public int? UserUnosId { get; set; }
        public bool? NavOK { get; set; }

        public virtual VozniParkDnevnikTip VozniParkDnevnikTip { get; set; }
        public virtual VozniPark VozniPark { get; set; }
        public virtual Region Region { get; set; }
        public virtual Artikli Artikli { get; set; }
        public virtual MagacinSpisak MagacinSpisak { get; set; }
    }
}
