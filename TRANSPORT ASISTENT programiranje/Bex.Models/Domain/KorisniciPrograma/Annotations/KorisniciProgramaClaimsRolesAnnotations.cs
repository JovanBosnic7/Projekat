using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(KorisniciProgramaClaimsRolesMetadata))]
    public partial class KorisniciProgramaClaimsRoles
    {
        public class KorisniciProgramaClaimsRolesMetadata
        {

            public int Id { get; set; }
            public int ClaimId { get; set; }
            public string RoleId { get; set; }
            

            
            private KorisniciProgramaClaimsRolesMetadata() { }
        }
    }
}
