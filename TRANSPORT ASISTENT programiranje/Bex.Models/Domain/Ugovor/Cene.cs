namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Cene

    {

        public int IdCene { get; set; }
        public int? CenovniRazredId { get; set; }

        public int? VrstaUslugeId { get; set; }
        //public int? KategorijaId { get; set; }
        public decimal? CenaMin { get; set; }
        public decimal? CenaProc { get; set; }
        public decimal? PopustProc { get; set; }
        public bool Nevazece { get; set; }

        //public virtual PosiljkaKategorija PosiljkaKategorija { get; set; }
        public virtual Cenovnik Cenovnik { get; set; }
    }
}
