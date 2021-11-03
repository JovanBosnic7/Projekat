namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class ArtikliVrsta
    {
        public int Id { get; set; }
        public string OznakaVrste { get; set; }
        public string NazivVrste { get; set; }
        public string NazivVrsteNav { get; set; }
        public bool? Storno { get; set; }

    }
}
