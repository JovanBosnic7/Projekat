using AspNet.DAL.EF.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KurirZaduzenje
    {
 
        public int Id { get; set; }
        public DateTime DatumZaduzenja { get; set; }
        public TimeSpan VremeZaduzenja { get; set; }
        public int? KurirUserId { get; set; }
        public int? ReonId { get; set; }
        public int? ZonaId { get; set; }

        public virtual KorisniciPrograma User { get; set; }
        public virtual Reon Reon { get; set; }
        public virtual Zona Zona { get; set; }

    }
}
