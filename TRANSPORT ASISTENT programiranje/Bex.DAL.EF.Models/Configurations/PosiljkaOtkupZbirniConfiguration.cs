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
    public class PosiljkaOtkupZbirniConfiguration: EntityTypeConfiguration<PosiljkaOtkupZbirni>
    {
        public PosiljkaOtkupZbirniConfiguration()
        {
            ToTable("PosiljkaOtkupZbirni");

            HasKey(z => z.Id);

            HasOptional(e => e.Reon)
            .WithMany(e => e.PosiljkaOtkupZbirni)
            .HasForeignKey(e => e.ReonId)
            .WillCascadeOnDelete(false);

            //HasOptional(e => e.User)
            //.WithMany(e => e.PosiljkaOtkupZbirni)
            //.HasForeignKey(e => e.IzvrsioId)
            //.WillCascadeOnDelete(false);

        }
    }
}
