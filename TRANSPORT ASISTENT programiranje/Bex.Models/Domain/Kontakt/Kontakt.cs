namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Kontakt
    {
 
        public int Id { get; set; }

       
        public string Ime { get; set; }

        public string Prezime { get; set; }

        
        public string Naziv { get; set; }

        public bool PravnoLice { get; set; }

        public bool Stranac { get; set; }

        public int? PakId { get; set; }

        public string BrojTxt { get; set; }

        public string AdresaTekst { get; set; }

        //public int AdresaId { get; set; }

        public int FirmaUnosId { get; set; } = 1;

        public int? UserUnosId { get; set; }


        //public virtual AdresaZaIzbor Adresa { get; set; }


        public virtual ICollection<KontaktAdresa> KontaktAdresa { get; set; }
        //public virtual ICollection<KontaktFizickoLice> Fizicko { get; set; }
        //public virtual ICollection<KontaktPravnoLice> Pravno { get; set; }
        public virtual ICollection<KontaktTelefon> Telefon { get; set; }
        public virtual ICollection<KontaktEmail> Email { get; set; }
        public virtual ICollection<KontaktZiroRacun> ZiroRacun { get; set; }
        public virtual ICollection<KontaktDelatnost> Delatnost { get; set; }
        //public virtual ICollection<Zaposleni> Zaposleni { get; set; }
        public virtual ICollection<Trebovanje> Trebovanje { get; set; }
        public virtual ICollection<PosiljkaZadatak> PosiljkaZadatak { get; set; } //tetka zakomentarisala, ccko vratio. treba pogledati

        //public virtual ICollection<KorisniciPrograma> User { get; set; }

        //public virtual ICollection<Ugovor> Ugovor { get; set; }


    }
}
