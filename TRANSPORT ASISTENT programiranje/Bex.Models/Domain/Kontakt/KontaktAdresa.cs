using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KontaktAdresa
    {
        public int Id { get; set; }

        public int KontaktId { get; set; }

        public int PakId { get; set; }

        public string AdresaTxt { get; set; }

        public string BrojTxt { get; set; }

        public int UserUnosId { get; set; }

        public virtual Kontakt Kontakt { get; set; }
        //public virtual Adresa Adresa { get; set; }
    }
}
