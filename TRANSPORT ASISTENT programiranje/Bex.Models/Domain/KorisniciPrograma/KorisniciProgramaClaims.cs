using AspNet.DAL.EF.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KorisniciProgramaClaims
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public string Description { get; set; }

        public string Filter { get; set; }
        

    }
}
