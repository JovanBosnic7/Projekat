using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BexMVC.Models
{
    public class BexMVCContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BexMVCContext() : base("name=BexMVCContext")
        {
        }

        public System.Data.Entity.DbSet<Bex.Models.Posiljka> Posiljkas { get; set; }

        public System.Data.Entity.DbSet<Bex.Models.PosiljkaKategorija> PosiljkaKategorijas { get; set; }

        public System.Data.Entity.DbSet<Bex.Models.PosiljkaSadrzaj> PosiljkaSadrzajs { get; set; }

        public System.Data.Entity.DbSet<Bex.Models.PosiljkaStatus> PosiljkaStatus { get; set; }

        public System.Data.Entity.DbSet<Bex.Models.PosiljkaVrsta> PosiljkaVrstas { get; set; }
    }
}
