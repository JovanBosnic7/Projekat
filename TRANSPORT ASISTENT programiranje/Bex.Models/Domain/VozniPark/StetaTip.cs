using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class StetaTip
    {

        public int Id { get; set; }
        public int KategorijaId { get; set; }
        public string NazivTipa { get; set; }
        public bool Storno { get; set; }

    }
}
