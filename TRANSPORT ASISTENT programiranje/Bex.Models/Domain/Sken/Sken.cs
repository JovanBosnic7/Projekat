namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Sken
    {
        public int Id { get; set; }
        public int? SkenStartId { get; set; }
        public string BarKod { get; set; }
        public DateTime? DatumUpisaNaServer { get; set; }
        public DateTime? DatumSkeniranjaNaPDA { get; set; }
        public virtual SkenStart SkenStart { get; set; }

    }
}
