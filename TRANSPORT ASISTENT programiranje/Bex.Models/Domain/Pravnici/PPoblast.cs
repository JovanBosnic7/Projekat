namespace Bex.Models
{
    using System;
    using System.Collections.Generic;

    public  partial class PPoblast
    {       
        public int Id { get; set; }        
        public string Naziv { get; set; }
        public bool Tuzeni { get; set; }
        public bool Tuzilac { get; set; }
       
    }
}
