namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class DimenzijeNav
    {
        public int Id { get; set; }
        public int? TipId { get; set; }
        public string NavCode { get; set; }
        public string Naziv { get; set; }
        public int? TipRedaId { get; set; }       
        public bool? Storno { get; set; }

    }
}
