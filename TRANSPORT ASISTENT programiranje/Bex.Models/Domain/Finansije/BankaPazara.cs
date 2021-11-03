namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class BankaPazara

    {
       
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public int? RegionId { get; set; }
        public decimal? PazarZaUplatu { get; set; }
        public decimal? PazarUplacen { get; set; }
        public decimal? OtkupZaUplatu { get; set; }
        public decimal? OtkupUplacen { get; set; }
        public decimal? OtkupZaIsplatu { get; set; }
        public decimal? OtkupIsplacen { get; set; }

        public virtual Region Region { get; set; }
    }
}
