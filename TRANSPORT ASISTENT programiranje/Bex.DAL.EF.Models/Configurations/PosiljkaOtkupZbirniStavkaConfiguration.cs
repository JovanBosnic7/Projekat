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
    public class PosiljkaOtkupZbirniStavkaConfiguration: EntityTypeConfiguration<PosiljkaOtkupZbirniStavka>
    {
        public PosiljkaOtkupZbirniStavkaConfiguration()
        {
            ToTable("PosiljkaOtkupZbirniStavka");

            HasKey(z => z.Id);

            //HasRequired(c => c.PosiljkaOtkupZbirni)
            //.WithMany(p => p.PosiljkaOtkupZbirniStavka)
            //.HasForeignKey(c => c.BarKod)
            //.WillCascadeOnDelete();
        }
    }
}
