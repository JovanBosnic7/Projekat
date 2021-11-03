using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class PosiljkaOtkupZbirniStavka
    {
 
        public int Id { get; set; }
        public string BarKod { get; set; }
        public int? PosiljkaId { get; set; }

        public virtual PosiljkaOtkupZbirni PosiljkaOtkupZbirni { get; set; }

    }
}
