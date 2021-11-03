namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class VozniParkKaroserija
    {
       
        public int Id { get; set; }
        public int KategorijaId { get; set; }
        public string NazivKaroserije { get; set; }
    }
}
