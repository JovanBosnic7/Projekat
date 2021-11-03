namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class BankaPazaraSpecifikacija

    {
       
        public int Id { get; set; }
        public DateTime DatumPazara { get; set; }
        public string TipZadatka { get; set; }
        public int? RegionUplateId { get; set; }
        public decimal PazarZaUplatu { get; set; }
        public decimal OtkupZaUplatu { get; set; }
        public int? RegionIsplateId { get; set; }
        public decimal OtkupZaIsplatu { get; set; }

    }
}
