using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class CekoviConfiguration: EntityTypeConfiguration<Cekovi>
    {
        public CekoviConfiguration()
        {
            ToTable("Cekovi");

            HasKey(e => e.Id);

            //Property(e => e.Id)
            //     .HasColumnName("BrojCeka");

        }
    }
}