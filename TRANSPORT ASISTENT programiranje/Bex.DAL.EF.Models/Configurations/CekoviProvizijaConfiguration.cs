using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class CekoviProvizijaConfiguration: EntityTypeConfiguration<CekoviProvizija>
    {
        public CekoviProvizijaConfiguration()
        {
            ToTable("CekoviProvizija");

            HasKey(e => e.Id);
            
        }
    }
}