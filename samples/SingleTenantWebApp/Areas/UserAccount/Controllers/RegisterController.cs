﻿using BrockAllen.MembershipReboot;
using CIC.IdentityManager.Web.Areas.UserAccount.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CIC.IdentityManager.Web.Areas.UserAccount.Controllers
{
    [AllowAnonymous]
    public class RegisterController : Controller
    {
        UserAccountService userAccountService;
        
        public RegisterController(UserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        public ActionResult Index()
        {
            return View(new RegisterInputModel());
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(RegisterInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.userAccountService.CreateAccount(model.Username, model.Password, model.Email);
                    if (userAccountService.Configuration.SecuritySettings.RequireAccountVerification)
                    {
                        return View("Success", model);
                    }
                    else
                    {
                        return View("Confirm", true);
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        public ActionResult Confirm(string id)
        {
            var result = this.userAccountService.VerifyAccount(id);
            return View("Confirm", result);
        }
        
        public ActionResult Cancel(string id)
        {
            var result = this.userAccountService.CancelNewAccount(id);
            return View("Cancel", result);
        }
    }
}
