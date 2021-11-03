using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class ZaposleniNapomenaConfiguration: EntityTypeConfiguration<ZaposleniNapomena>
    {
        public ZaposleniNapomenaConfiguration()
        {
            ToTable("ZaposleniNapomena");

            HasKey(e => e.Id);

            

            //HasOptional(e => e.Zaposleni)
            //.WithMany(e => e.ZaposleniNapomena)
            //.HasForeignKey(e => e.ZaposleniId)
            //.WillCascadeOnDelete(false);
        }
    }
}