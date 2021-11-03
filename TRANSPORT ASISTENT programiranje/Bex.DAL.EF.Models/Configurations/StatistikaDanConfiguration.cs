using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class StatistikaDanConfiguration: EntityTypeConfiguration<StatistikaDan>
    {
        public StatistikaDanConfiguration()
        {
            ToTable("StatistikaDan");

            HasKey(e => e.Id);
            
        }
    }
}