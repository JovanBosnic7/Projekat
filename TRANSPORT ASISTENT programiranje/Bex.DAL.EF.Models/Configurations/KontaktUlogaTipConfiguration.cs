using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class KontaktUlogaTipConfiguration : EntityTypeConfiguration<KontaktUlogaTip>
    {
        public KontaktUlogaTipConfiguration()
        {
            ToTable("KontaktUlogaTip");

            HasKey(e => e.Id);
            
        }
    }
}