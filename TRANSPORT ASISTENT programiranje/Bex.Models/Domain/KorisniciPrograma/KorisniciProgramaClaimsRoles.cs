using AspNet.DAL.EF.Models.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KorisniciProgramaClaimsRoles
    {
       
        public int Id { get; set; }
        public int ClaimId { get; set; }
        public string RoleId { get; set; }
        

        

    }
}
