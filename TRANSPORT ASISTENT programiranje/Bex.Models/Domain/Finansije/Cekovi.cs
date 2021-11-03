namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Cekovi

    {
       
        public int Id { get; set; }        
        public string BrojCeka { get; set; }
        public DateTime DatumDospeca { get; set; }
        public string BrojTekucegRacuna { get; set; }
        public int IznosCeka { get; set; }
        public int UserUneoId { get; set; }
        public DateTime DatumUnosa { get; set; }
        public DateTime? DatumRealizacije { get; set; }
        public int BankaId { get; set; }
        public int BrojSpecifikacije { get; set; }
        public bool Storno { get; set; }
        public string Napomena { get; set; }
        public bool InternoRazduzen { get; set; }
        public int? ZastupnikId { get; set; }
        public int ProvizijaId { get; set; }
        public virtual Banke Banka { get; set; }
        public virtual KontaktUloga KontaktUloga { get; set; }
        public virtual CekoviProvizija CekProvizija { get; set; }
        public virtual KorisniciPrograma UserUneo { get; set; }
    }
}
