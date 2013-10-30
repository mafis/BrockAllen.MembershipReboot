﻿using BrockAllen.MembershipReboot;
using CIC.IdentityManager.Web.App_Start;
using System.Data.Entity;
using System.Security.Claims;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CIC.IdentityManager.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        void Application_BeginRequest()
        {
            InitDatabase();
        }

        private void InitDatabase()
        {
            var svc = DependencyResolver.Current.GetService<UserAccountService>();
            if (svc.GetByUsername("admin") == null)
            {
                var account = svc.CreateAccount("admin", "admin123", "brockallen@gmail.com");
                svc.VerifyAccount(account.VerificationKey);
                    
                account = svc.GetByID(account.ID);
                account.AddClaim(ClaimTypes.Role, "Administrator");
                account.AddClaim(ClaimTypes.Role, "Manager");
                account.AddClaim(ClaimTypes.Role, "IdentityServerAdministrators");                
                account.AddClaim(ClaimTypes.Country, "USA");
                svc.Update(account);
            }
        }
    }
}