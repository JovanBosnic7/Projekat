using System.Collections.Generic;
using Bex.Models;


namespace BexMVC.ViewModels
{
   public class ReonIndexData
   {
        public IEnumerable<Reon> Reon { get; set; }
        public Region Region { get; set; }
        public ReonTip ReonTip { get; set; }

        public int ReonId { get; set; }
        public string OznakaReona { get; set; }

        public string NazivReona { get; set; }

        public string NazivRegiona { get; set; }

        public string TipReona { get; set; }

        public string BarkodReona { get; set; }

        public int? KmReona { get; set; }
        public int? KmOptimalna { get; set; }


    }
}