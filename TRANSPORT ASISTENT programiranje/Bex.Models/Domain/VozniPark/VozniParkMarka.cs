namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class VozniParkMarka
    {
       
        public int Id { get; set; }
        public string NazivMarke { get; set; }

        //public virtual ICollection<VozniParkModel> VozniParkModel { get; set; }

    }
}
