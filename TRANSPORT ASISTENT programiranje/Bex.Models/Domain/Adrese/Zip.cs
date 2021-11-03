namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class Zip
    {
       
        public int Id { get; set; }

        public int? ZipValue { get; set; }

       

        public virtual ICollection<PAK> PAKs { get; set; }
    }
}
