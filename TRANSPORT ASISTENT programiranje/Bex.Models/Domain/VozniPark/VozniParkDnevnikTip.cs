using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class VozniParkDnevnikTip
    {
 
        public int Id { get; set; }
        public int GrupaId { get; set; }
        public string NazivTipa { get; set; }    
        
        public int? DanaIstekaDefault { get; set; }
        public int? KmIstekaDefault { get; set; }

    }
}
