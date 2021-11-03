namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class VozniParkKategorija
    {
       
        public int Id { get; set; }

        public string KategorijaNaziv { get; set; }

        public string NazivPodkategorije { get; set; }
        public int Sort { get; set; }
    }
}
