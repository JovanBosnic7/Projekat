namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public  partial class ReonTip
    {
       
        public int Id { get; set; }

        public string Opis { get; set; }

        public string OpisPom { get; set; }
        public virtual ICollection<Reon> Reons { get; set; }

    }
}
