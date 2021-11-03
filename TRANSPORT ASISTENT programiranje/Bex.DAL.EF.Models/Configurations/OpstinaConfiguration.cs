using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class OpstinaConfiguration : EntityTypeConfiguration<Opstina>
    {
        public OpstinaConfiguration()
        {
            HasKey(e => e.Id);

            ToTable("Opstina");            

            Property(e => e.Id)
                .HasColumnName("IdOpstine");
        }
    }
}