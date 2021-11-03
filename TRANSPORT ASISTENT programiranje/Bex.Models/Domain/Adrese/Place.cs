namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Place
    {
        public int Id { get; set; }

        public string PlaceName { get; set; }

        public int? OpstinaId { get; set; }
        public string Ptt { get; set; }

        public string OznakaMesta { get; set; }


        public virtual Opstina Opstina { get; set; }

        public virtual ICollection<Street> Streets { get; set; }

        public virtual ICollection<PAK> PAKs { get; set; }
    }
}
