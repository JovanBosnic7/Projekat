namespace Bex.Models
{
    using System;
    using System.Collections.Generic;


    public  partial class Reoncic
    {
        public int Id { get; set; }

        public int? ReonId { get; set; }

        public string NazivReoncica { get; set; }

        public TimeSpan? PreuzimanjeDoDefault { get; set; }

        public bool OdjavljujeSe { get; set; }

        public DateTime? DatumPoslednjeOdjave { get; set; }

        public TimeSpan? VremePoslednjeOdjave { get; set; }

        public int? Sort { get; set; }

        public bool DeoMesta { get; set; }

        public string NapomenaOdjave { get; set; }

        public virtual Reon Reon { get; set; }
        public virtual ICollection<PAK> PAKs { get; set; }
    }
}
