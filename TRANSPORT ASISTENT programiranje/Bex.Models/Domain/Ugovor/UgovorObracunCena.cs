namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class UgovorObracunCena
    {       
        public int Id { get; set; }
        public int? UgovorId { get; set; }
        public int? PovlasceneCeneKlijentuId { get; set; }
        public int? NaplataId { get; set; }
        public bool BezZastitneCene { get; set; }
        public int? ZastitnaCena { get; set; }
        public bool NaplataPoFakturi { get; set; }
        public int? FakturaProc { get; set; }
        public int? ZastitnaCenaFak { get; set; }
        public int? DodatakZaGorivo { get; set; }      
        public DateTime DatumUnosa { get; set; }

        public virtual Ugovor Ugovor { get; set; }
        public virtual UgovorNacinNaplate UgovorNacinNaplate { get; set; }
        public virtual UgovorPovlascenaCenaTip UgovorPovlascenaCenaTip { get; set; }
    }
}
