using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.DAL.EF.Models.Security;
using AspNet.DAL.EF.UOW.Security.Repositories;
using Bex.Common.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace AspNet.DAL.EF.UOW.Security.Repositories
{
    public class ApplicationSignInManager :
        SignInManager<ApplicationUser, string>, ISignInRepository
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        { }

        public static ApplicationSignInManager Create(
            IdentityFactoryOptions<ApplicationSignInManager> options,
            IOwinContext context) =>
            new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);

        public async override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user) =>
            await user.CreateUserIdentityAsync((ApplicationUserManager)UserManager);

        public async Task SignInUserAsync(IUser user, bool isPersistent, bool rememberBrowser) =>
            await base.SignInAsync((ApplicationUser)user, isPersistent, rememberBrowser);

        public Task<bool> TwoFactorBrowserRememberedAsync(string userId) =>
            AuthenticationManager.TwoFactorBrowserRememberedAsync(userId);
    }
}
