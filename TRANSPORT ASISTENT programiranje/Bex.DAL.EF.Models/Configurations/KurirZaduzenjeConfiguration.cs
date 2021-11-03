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
    public class KurirZaduzenjeConfiguration : EntityTypeConfiguration<KurirZaduzenje>
    {
        public KurirZaduzenjeConfiguration()
        {
            ToTable("KurirZaduzenje");

            HasKey(z => z.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("IdZaduzenja");

            //HasOptional(e => e.User)
            //.WithMany(e => e.KurirZaduzenje)
            //.HasForeignKey(e => e.KurirUserId)
            //.WillCascadeOnDelete(false);

            HasOptional(e => e.Reon)
            .WithMany(e => e.KurirZaduzenje)
            .HasForeignKey(e => e.ReonId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.Zona)
            .WithMany(e => e.KurirZaduzenje)
            .HasForeignKey(e => e.ZonaId)
            .WillCascadeOnDelete(false);

        }
    }
}
