using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class VpOpremaGrupaConfiguration: EntityTypeConfiguration<VpOpremaGrupa>
    {
        public VpOpremaGrupaConfiguration()
        {
            ToTable("VpOpremaGrupa");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdGrupe");
        }
    }
}