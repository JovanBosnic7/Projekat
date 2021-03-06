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
    public class ZaposleniPlataConfiguration: EntityTypeConfiguration<ZaposleniPlata>
    {
        public ZaposleniPlataConfiguration()
        {
            ToTable("zaposleniPlata");

            HasKey(z => z.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("IdPlate");

            Property(p => p.ZaposleniId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
                .HasColumnName("ZaposleniId");

            

            Property(z => z.ZaposleniId)
                .IsRequired();
        }
    }
}
