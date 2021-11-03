using System;
using AspNet.DAL.EF.UOW.Security.Repositories;
using Bex.Common.Interfaces;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace AspNet.DAL.EF.UOW.Security
{
    public partial class SecurityUow : ISecurityUow, IDisposable
    {
        public SecurityUow(IOwinContext owinContext)
        {
            SignInManager = owinContext.Get<ApplicationSignInManager>();
            UserManager = owinContext.GetUserManager<ApplicationUserManager>();
            RoleManager = owinContext.Get<ApplicationRoleManager>();
            

        }

        public ISignInRepository SignInManager { get; private set; }
        public IUserRepository UserManager { get; private set; }
        public IRoleRepository RoleManager { get; private set; }

       

        public void Dispose()
        {
            if (SignInManager != null)
            { SignInManager.Dispose(); }

            if (UserManager != null)
            { UserManager.Dispose(); }

            if (RoleManager != null)
            { RoleManager.Dispose(); }
        }
    }
}
