using System;
using AspNet.DAL.EF.Models.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AspNet.DAL.EF.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("BexMainDbContext", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
        }

        public static ApplicationDbContext Create()
        { return new ApplicationDbContext(); }
    }
}
