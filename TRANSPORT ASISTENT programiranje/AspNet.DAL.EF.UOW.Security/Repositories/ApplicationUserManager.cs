using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class ApplicationUserManager :
        UserManager<ApplicationUser>, IUserRepository
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        { }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers.
            // This application uses Phone and Emails (if they are confirmed) as a step
            //  of receiving a code for verifying the user.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        public IUser CreateUser(string email, string userName) =>
           new ApplicationUser { UserName = userName, Email = email};

        //public IUser CreateUser(string email, string favoriteColor) =>
        //    new ApplicationUser { UserName = email, Email = email, FavoriteColor = favoriteColor };

        //public IUser CreateUser(int? kontaktId, int? regionId, string barkod, bool? klijent, bool? aktivan) =>
        //    new ApplicationUser { KontaktId = kontaktId, RegionId = regionId, Barkod = barkod, Klijent = klijent, Aktivan = aktivan };


        //public IUser CreateUser(string email) =>
        //new ApplicationUser { UserName = email, Email = email };

        public async Task<IdentityResult> DeleteAsync(IUser user) =>
        await base.DeleteAsync((ApplicationUser)user);

        public async Task<IdentityResult> RegisterUserAsync(IUser user) =>
            await base.CreateAsync((ApplicationUser)user);

        public async Task<IdentityResult> RegisterUserAsync(IUser user, string password) =>
            await base.CreateAsync((ApplicationUser)user, password);

        public async Task<IUser> FindUserByIdAsync(string userId) =>
            await FindByIdAsync(userId);

        public async Task<IUser> FindUserByEmailAsync(string userName) =>
            await FindByNameAsync(userName);

        public async Task<string> GetUserPasswordHashAsync(string userId) =>
            (await FindByIdAsync(userId)).PasswordHash;

        
        public Task<List<ApplicationUser>> GetUsersAsync() =>
            Task.FromResult(Users.ToList().Select(item => item as ApplicationUser).ToList());

        public async Task<bool> DeleteClaim(string userId, int claimId)
        {
            var appUser = await FindByIdAsync(userId);
            var claim = appUser.Claims.Select(x => x.Id == claimId) as IdentityUserClaim;
            appUser.Claims.Remove(claim);
            return true;
        }

        
    }
}
