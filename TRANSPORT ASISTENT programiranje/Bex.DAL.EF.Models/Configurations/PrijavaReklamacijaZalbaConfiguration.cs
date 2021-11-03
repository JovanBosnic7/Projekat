using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class PrijavaReklamacijaZalbaConfiguration : EntityTypeConfiguration<PrijavaReklamacijaZalba>
    {
        public PrijavaReklamacijaZalbaConfiguration()
        {
            ToTable("PrijavaReklamacijaZalba");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("Id");

            

        }
    }
}