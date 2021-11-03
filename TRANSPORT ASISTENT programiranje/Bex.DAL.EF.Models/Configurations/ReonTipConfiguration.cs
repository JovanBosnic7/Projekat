using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class ReonTipConfiguration : EntityTypeConfiguration<ReonTip>
    {
        public ReonTipConfiguration()
        {
            ToTable("ReonTip");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdTip");            

            
        }
    }
}