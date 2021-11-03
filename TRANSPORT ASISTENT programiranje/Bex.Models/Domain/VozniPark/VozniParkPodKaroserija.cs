namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class VozniParkPodKaroserija
    {
       
        public int Id { get; set; }
        public int KaroserijaId { get; set; }
        public string NazivPodKaroserije { get; set; }
    }
}
