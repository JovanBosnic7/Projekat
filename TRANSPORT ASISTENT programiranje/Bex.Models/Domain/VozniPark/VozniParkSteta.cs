using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class VozniParkSteta
    {

        public int Id { get; set; }
        public int? FirmaSteteId { get; set; }
        public int? ZavisnaTabelaId { get; set; }
        public int? NalogIzdaoID { get; set; }
        public int? NalogSektorId { get; set; }
        public int? KategorijaId { get; set; }
        public string Opis { get; set; }
        public string Napomena { get; set; }
        public int? StetocinaZaposleniId { get; set; }
        public int? StetocinaCentarId { get; set; }
        public DateTime? DatumPredajePravnoj { get; set; }
        public int? IznosRsd { get; set; }
        public int? IznosZaNaplatu { get; set; }
        public DateTime? DatumUnosa { get; set; }
        public int? UserUnosId { get; set; }
        public bool? Sporno { get; set; }
        public bool? Racun { get; set; }
        public bool? Kes { get; set; }
        public bool? Storno { get; set; }
        public DateTime? DatumOdluke { get; set; }
        public bool? KnjigovodstveniManjak { get; set; }
        public int? ValutaId { get; set; }
        public bool? PotpisanaOdluka { get; set; }
        public bool? Nenaplativo { get; set; }


        public virtual FirmaVP Firma { get; set; }
        public virtual VozniPark VozniPark { get; set; }
        public virtual KorisniciPrograma KorisnikIzdao { get; set; }
        public virtual StetaKategorija StetaKategorija { get; set; }
        public virtual StetaTip StetaTip { get; set; }
        public virtual Zaposleni StetocinaZaposleni { get; set; }
        public virtual Region StetocinaCentar { get; set; }
        public virtual KorisniciPrograma KorisnikUneo { get; set; }

    }
}
