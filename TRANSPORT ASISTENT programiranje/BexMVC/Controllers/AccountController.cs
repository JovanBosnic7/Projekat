using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AspNet.DAL.EF.UOW.Security;
using Bex.Common.Interfaces;
using BexMVC.Helpers;
using BexMVC.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace BexMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        public AccountController() :
            this(new SecurityUow(System.Web.HttpContext.Current.GetOwinContext()))
        { }
        public AccountController(ISecurityUow securityUow)
        {
            SecurityUow = securityUow;
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind(Include = "Email,Password,RememberMe")] LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await SecurityUow.SignInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, true);

                

                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(returnUrl);
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { returnUrl });
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        { return View(); }

        public ActionResult RegisterKorisniciPrograma()
        { return View(); }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include = "Email,Password,ConfirmPassword, UserName")] RegisterViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = SecurityUow.UserManager.CreateUser(model.Email, model.UserName); 
                var result = await SecurityUow.UserManager.RegisterUserAsync(user, model.Password);
                if (result.Succeeded)
                {
                    if (String.IsNullOrEmpty(returnUrl))
                    {
                        await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToAction("../Home");
                        //return RedirectToAction("EmailConfirmation", "Manage"); kad se uvede verifikacija emaila
                    }
                    else if (returnUrl.Equals("CreateKorisniciPrograma"))
                    {
                        return RedirectToAction("../User/Edit", new { id = user.Id, returnUrl = "KorisniciPrograma" });
                    }
                    
                }

                foreach (var error in result.Errors)
                { ModelState.AddModelError("", error); }
            }

            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        { return View(); }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword([Bind(Include = "Email")] ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await SecurityUow.UserManager.FindUserByEmailAsync(model.Email);
                if (user == null) //|| !(await SecurityUow.UserManager.IsEmailConfirmedAsync(user.Id))
                // Don't reveal that the user does not exist or is not confirmed
                { return View("Error"); }

                string code = await SecurityUow.UserManager.GeneratePasswordResetTokenAsync(user.Id);

                //email confirm
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: Request.Url.Scheme);
                // next line will do nothing if email service isn't registered as UserManager.EmailService
                //await SecurityUow.UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");

                //TODO: in production delete next line
                //ViewBag.Link = callbackUrl;
                //return View("ForgotPasswordConfirmation");
                return RedirectToAction("ResetPassword", "Account", new { userId = user.Id, code });
            }

            return View(model); 
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        { return code == null ? View("Error") : View(); }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword([Bind(Include = "Email,Password,ConfirmPassword,Code")] ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            { return View(model); }

            var user = await SecurityUow.UserManager.FindUserByEmailAsync(model.Email);
            if (user == null)
            // Don't reveal that the user does not exist
            { return RedirectToAction("ConfirmedResetPassword"); }

            var result = await SecurityUow.UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            { return RedirectToAction("ConfirmedResetPassword"); }

            foreach (var error in result.Errors)
            { ModelState.AddModelError("", error); }

            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmedResetPassword()
        { return View(); }

        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl)
        {
            var userId = await SecurityUow.SignInManager.GetVerifiedUserIdAsync();
            if (userId != null)
            {
                var userFactors = await SecurityUow.UserManager.GetValidTwoFactorProvidersAsync(userId);
                var factorOptions = userFactors.Select(purpose =>
                    new SelectListItem { Text = purpose, Value = purpose })
                    .ToList();

                return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl });
            }

            return View("Error");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode([Bind(Include = "SelectedProvider,ReturnUrl")] SendCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await SecurityUow.SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
                { return RedirectToAction("VerifyCode", new { provider = model.SelectedProvider, returnUrl = model.ReturnUrl }); }

                return View("Error");
            }

            return View();
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (await SecurityUow.SignInManager.HasBeenVerifiedAsync())
            {

                var user = await SecurityUow.UserManager.FindUserByIdAsync(
                    await SecurityUow.SignInManager.GetVerifiedUserIdAsync());
                if (user != null)
                {
                    var code = await SecurityUow.UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);

                    //TODO: in production delete next 2 line(s)
                    ViewBag.Provider = provider;
                    ViewBag.Code = code;
                }

                return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
            }

            return View("Error");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode([Bind(Include = "Provider,Code,ReturnUrl,RememberBrowser")] VerifyCodeViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await SecurityUow.SignInManager.TwoFactorSignInAsync(
                    model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
                switch (result)
                {
                    case SignInStatus.Success:
                        return RedirectToLocal(model.ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid code.");
                        return View(model);
                }
            }

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(
                provider,
                Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            { return RedirectToAction("Login"); }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SecurityUow.SignInManager.ExternalSignInAsync(loginInfo, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            { return RedirectToAction("Index", "Manage"); }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                { return View("ExternalLoginFailure"); }

                var user = SecurityUow.UserManager.CreateUser(model.Email, null); 
                var result = await SecurityUow.UserManager.RegisterUserAsync(user);
                if (result.Succeeded)
                {
                    result = await SecurityUow.UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SecurityUow.SignInManager.SignInUserAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                { ModelState.AddModelError("", error); }
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            { return Redirect(returnUrl); }

            return RedirectToAction("Index", "Home");
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