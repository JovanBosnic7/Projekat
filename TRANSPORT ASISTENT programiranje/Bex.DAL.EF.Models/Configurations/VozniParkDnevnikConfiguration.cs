using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class VozniParkDnevnikConfiguration: EntityTypeConfiguration<VozniParkDnevnik>
    {
        public VozniParkDnevnikConfiguration()
        {
            ToTable("VpDnevnik");

            HasKey(z => z.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("IdVpDnevnik");


            //HasOptional(e => e.VozniPark)
            //   .WithMany(e => e.VozniParkDnevnik)
            //   .HasForeignKey(e => e.VpId)
            //   .WillCascadeOnDelete(false);

            //HasOptional(e => e.Region)
            //   .WithMany(e => e.VozniParkDnevnik)
            //   .HasForeignKey(e => e.RegionId)
            //   .WillCascadeOnDelete(false);

        }
    }
}
