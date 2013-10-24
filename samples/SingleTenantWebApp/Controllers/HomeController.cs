using System.Web.Mvc;

namespace CIC.IdentityManager.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Login", new { area = "UserAccount" });
        }

    }
}
