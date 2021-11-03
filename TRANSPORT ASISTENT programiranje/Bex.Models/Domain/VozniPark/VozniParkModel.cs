namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class VozniParkModel
    {
       
        public int Id { get; set; }
        public string NazivModela { get; set; }
        public int MarkaId { get; set; }

        public virtual VozniParkMarka VozniParkMarka { get; set; }
    }
}
