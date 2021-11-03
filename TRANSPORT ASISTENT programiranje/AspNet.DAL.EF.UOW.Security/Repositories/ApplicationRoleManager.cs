using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNet.DAL.EF.Contexts;
using AspNet.DAL.EF.Models.Security;
using Bex.Common.Interfaces;
using Bex.DAL.EF.Contexts.Main;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace AspNet.DAL.EF.UOW.Security.Repositories
{
    public class ApplicationRoleManager :
        RoleManager<ApplicationRole>, IRoleRepository
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store)
            : base(store)
        { }

        public static ApplicationRoleManager Create(
            IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationRoleManager(
                new RoleStore<ApplicationRole>(context.Get<ApplicationDbContext>()));

            return manager;
        }

        public Task<List<IRole>> GetRolesAsync() =>
            Task.FromResult(Roles.ToList().Select(item => item as IRole).ToList());
    }
}
