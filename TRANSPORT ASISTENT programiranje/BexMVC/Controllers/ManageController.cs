using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AspNet.DAL.EF.Models.Security;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common.Interfaces;
using BexMVC.Helpers;
using BexMVC.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace BexMVC.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        public ManageController() :
            this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()))
        { }
        public ManageController(ISecurityUow securityUow)
        {
            SecurityUow = securityUow;
        }

        public async Task<ActionResult> Index(ManageMessage? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessage.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessage.ConfirmPhoneSuccess ? "Your phone number was confirmed."
                : message == ManageMessage.RemovePhoneSuccess ? "Your phone number was removed."
                : message == ManageMessage.ChangeTwoFactorSuccess ? "Your two-factor authentication provider has been changed."
                : message == ManageMessage.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessage.Error ? "An error has occurred."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = await SecurityUow.UserManager.HasPasswordAsync(userId),
                IsEmailConfirmed = await SecurityUow.UserManager.IsEmailConfirmedAsync(userId),
                PhoneNumber = await SecurityUow.UserManager.GetPhoneNumberAsync(userId),
                IsPhoneNumberConfirmed = await SecurityUow.UserManager.IsPhoneNumberConfirmedAsync(userId),
                TwoFactor = await SecurityUow.UserManager.GetTwoFactorEnabledAsync(userId),
                BrowserRemembered = await SecurityUow.SignInManager.TwoFactorBrowserRememberedAsync(userId),
                IsInAdmins = await SecurityUow.UserManager.IsInRoleAsync(userId, "Admins"),
                Logins = await SecurityUow.UserManager.GetLoginsAsync(userId)
            };

            return View(model);
        }

        public ActionResult ChangePassword()
        { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword([Bind(Include = "OldPassword,NewPassword,ConfirmPassword")] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            { return View(model); }

            var userId = User.Identity.GetUserId();
            var result = await SecurityUow.UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await SecurityUow.UserManager.FindUserByIdAsync(userId);
                if (user != null)
                { await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false); }

                return RedirectToAction("Index", new { Message = ManageMessage.ChangePasswordSuccess });
            }

            foreach (var error in result.Errors)
            { ModelState.AddModelError("", error); }

            return View(model);
        }

        public async Task<ActionResult> EmailConfirmation()
        {
            var userId = User.Identity.GetUserId();
            string code = await SecurityUow.UserManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = Url.Action("ConfirmedEmail", "Manage", new { userId, code }, protocol: Request.Url.Scheme);

            // next line will do nothing if email service isn't registered as UserManager.EmailService
            //await SecurityUow.UserManager.SendEmailAsync(userId, "Confirm your mail", "Please confirm your mail by clicking <a href=\"" + callbackUrl + "\">here</a>");

            //TODO: in production delete next line
            ViewBag.Link = callbackUrl;
            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmedEmail(string userId, string code)
        {
            if (userId == null || code == null)
            { return View("Error"); }

            var result = await SecurityUow.UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmedEmail" : "Error");
        }

        public ActionResult AddPhoneNumber()
        { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber([Bind(Include = "Number")] AddPhoneNumberViewModel model)
        {
            if (ModelState.IsValid)
            {
                await SecurityUow.UserManager.SetPhoneNumberAsync(
                    User.Identity.GetUserId(), model.Number);

                return RedirectToAction("ConfirmPhoneNumber", new { phoneNumber = model.Number });
            }

            return View(model);
        }

        public async Task<ActionResult> ConfirmPhoneNumber(string phoneNumber)
        {
            var userId = User.Identity.GetUserId();
            var code = await SecurityUow.UserManager.GenerateChangePhoneNumberTokenAsync(
                userId, phoneNumber);

            // next line will do nothing if sms service isn't registered as UserManager.SmsService
            await SecurityUow.UserManager.SendSmsAsync(userId, "Your security code is " + code);

            //TODO: in production delete next line
            ViewBag.Code = code;
            return phoneNumber == null ? View("Error")
                : View(new ConfirmPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmPhoneNumber([Bind(Include = "Code,PhoneNumber")] ConfirmPhoneNumberViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var result = await SecurityUow.UserManager.ChangePhoneNumberAsync(
                    userId, model.PhoneNumber, model.Code);

                var applicationUser = await SecurityUow.UserManager.FindUserByIdAsync(userId) as ApplicationUser;
                applicationUser.Claims.Clear();

               
                await SecurityUow.UserManager.RemoveClaimAsync(User.Identity.GetUserId(), new Claim("www.bex.rs/TIP", "AAA"));
                await SecurityUow.UserManager.AddClaimAsync(User.Identity.GetUserId(), new Claim("www.bex.rs/DALI", "XXX"));


                if (result.Succeeded)
                {
                    var user = await SecurityUow.UserManager.FindUserByIdAsync(userId);
                    if (user != null)
                    { await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false); }

                    return RedirectToAction("Index", new { Message = ManageMessage.ConfirmPhoneSuccess });
                }

                ModelState.AddModelError("", "Failed to verify phone");
                return View(model);
            }

            return View(model);
        }

        public async Task<ActionResult> RemovePhoneNumber()
        {
            var userId = User.Identity.GetUserId();
            var result = await SecurityUow.UserManager.SetPhoneNumberAsync(userId, null);
            if (result.Succeeded)
            {
                var user = await SecurityUow.UserManager.FindUserByIdAsync(userId);
                if (user != null)
                { await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false); }

                return RedirectToAction("Index", new { Message = ManageMessage.RemovePhoneSuccess });
            }

            return RedirectToAction("Index", new { Message = ManageMessage.Error });
        }

        [HttpPost]
        public async Task<ActionResult> EnableTFA()
        {
            var userId = User.Identity.GetUserId();
            await SecurityUow.UserManager.SetTwoFactorEnabledAsync(userId, true);

            var user = await SecurityUow.UserManager.FindUserByIdAsync(userId);
            if (user != null)
            { await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false); }

            return RedirectToAction("Index", new { Message = ManageMessage.ChangeTwoFactorSuccess });
        }

        [HttpPost]
        public async Task<ActionResult> DisableTFA()
        {
            var userId = User.Identity.GetUserId();
            await SecurityUow.UserManager.SetTwoFactorEnabledAsync(userId, false);

            var user = await SecurityUow.UserManager.FindUserByIdAsync(userId);
            if (user != null)
            { await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false); }

            return RedirectToAction("Index", new { Message = ManageMessage.ChangeTwoFactorSuccess });
        }

        public async Task<ActionResult> ManageLogins(ManageMessage? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessage.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessage.Error ? "An error has occurred."
                : "";

            var userId = User.Identity.GetUserId();
            var user = await SecurityUow.UserManager.FindUserByIdAsync(userId);
            if (user == null)
            { return View("Error"); }

            var userLogins = await SecurityUow.UserManager.GetLoginsAsync(userId);
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = SecurityUow.UserManager.GetUserPasswordHashAsync(userId) != null || userLogins.Count > 1;

            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        public ActionResult SetPassword()
        { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword([Bind(Include = "NewPassword,ConfirmPassword")] SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await SecurityUow.UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await SecurityUow.UserManager.FindUserByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    { await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false); }

                    return RedirectToAction("Index", new { Message = ManageMessage.SetPasswordSuccess });
                }

                foreach (var error in result.Errors)
                { ModelState.AddModelError("", error); }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessage? message;

            var userId = User.Identity.GetUserId();
            var result = await SecurityUow.UserManager.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await SecurityUow.UserManager.FindUserByIdAsync(userId);
                if (user != null)
                { await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false); }

                message = ManageMessage.RemoveLoginSuccess;
            }
            else
            { message = ManageMessage.Error; }

            return RedirectToAction("ManageLogins", new { Message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            return new ChallengeResult(
                provider,
                Url.Action("LinkLoginCallback", new { xsrfKey = "XsrfId" }),
                User.Identity.GetUserId(),
                "XsrfId");
        }

        public async Task<ActionResult> LinkLoginCallback(string xsrfKey)
        {
            var userId = User.Identity.GetUserId();
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(xsrfKey, userId);
            if (loginInfo == null)
            { return RedirectToAction("ManageLogins", new { Message = ManageMessage.Error }); }

            var result = await SecurityUow.UserManager.AddLoginAsync(userId, loginInfo.Login);
            return result.Succeeded ?
                RedirectToAction("ManageLogins") :
                RedirectToAction("ManageLogins", new { Message = ManageMessage.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (SecurityUow != null)
                { SecurityUow.Dispose(); }
            }
            base.Dispose(disposing);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        private ISecurityUow SecurityUow { get; set; }
    }
}
