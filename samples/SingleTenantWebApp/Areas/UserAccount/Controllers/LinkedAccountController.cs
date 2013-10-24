﻿using BrockAllen.MembershipReboot;
using BrockAllen.OAuth2;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CIC.IdentityManager.Web.Areas.UserAccount.Controllers
{
    public class LinkedAccountController : Controller
    {
        static LinkedAccountController()
        {
            RegisterOAuth2Clients();
        }

        static void RegisterOAuth2Clients()
        {
            OAuth2Client.Instance.RegisterProvider(
                ProviderType.Google,
                "464251281574.apps.googleusercontent.com",
                "najvdnYI5TjCkikCi6nApRu1");

            OAuth2Client.Instance.RegisterProvider(
                ProviderType.Facebook,
                "260581164087472",
                "7389d78e6e629954a710351830d080f3");

            //OAuth2Client.Instance.RegisterProvider(
            //    ProviderType.Live,
            //    "00000000400DF045",
            //    "4L08bE3WM8Ra4rRNMv3N--un5YOBr4gx");
        }

        AuthenticationService authenticationService;
        UserAccountService userAccountService;

        public LinkedAccountController(
            AuthenticationService AuthenticationService)
        {
            this.authenticationService = AuthenticationService;
            this.userAccountService = AuthenticationService.UserAccountService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(ProviderType type)
        {
            return new OAuth2ActionResult(type, Url.Action("Manage"));
        }

        [Authorize]
        public ActionResult Manage()
        {
            var linkedAccounts = this.userAccountService.GetByID(User.GetUserID()).LinkedAccounts.ToArray();
            return View("Manage", linkedAccounts);
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Remove(string provider, string id)
        {
            try
            {
                var account = this.userAccountService.GetByID(User.GetUserID());
                account.RemoveLinkedAccount(provider, id);
                this.userAccountService.Update(account);
                return RedirectToAction("Manage");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Manage();
        }


        public async Task<ActionResult> OAuthCallback()
        {
            try
            {
                var result = await OAuth2Client.Instance.ProcessCallbackAsync();
                if (result.Error == null)
                {
                    var provider = result.ProviderName;
                    var claims = result.Claims;
                    var id = claims.GetValue(ClaimTypes.NameIdentifier);

                    this.authenticationService.SignInWithLinkedAccount(provider, id, claims);

                    if (result.ReturnUrl != null)
                    {
                        return Redirect(result.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.Error);
                    if (!String.IsNullOrWhiteSpace(result.ErrorDetails))
                    {
                        ModelState.AddModelError("", result.ErrorDetails);
                    }
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error Signing In");
            }

            //return View("~/Areas/UserAccount/Views/LinkedAccount/SignInError.cshtml");
            return View("SignInError");
        }
    }
}
