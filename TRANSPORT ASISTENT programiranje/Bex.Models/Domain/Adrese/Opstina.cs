namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Opstina
    {
        public int Id { get; set; }

        public string OpstinaNaziv { get; set; }
        public virtual ICollection<Place> Place { get; set; }

    }
}
