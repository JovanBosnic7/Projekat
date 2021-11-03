namespace Bex.Models
{
    using AspNet.DAL.EF.Models.Security;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class KalendarPlaner
    {
 
        public int Id { get; set; }       
        public string Naziv { get; set; }
        public string Opis { get; set; }        
        public DateTime? DatumStart { get; set; }
        public DateTime? DatumEnd { get; set; }
        public int? UserId { get; set; }

        public string Color { get; set; }

    }
}
