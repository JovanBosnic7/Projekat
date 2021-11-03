namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class FilterSpisakKolona
    {
       
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int Filter { get; set; }
        public bool Selektovan { get; set; }
    }
}
