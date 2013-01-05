using System.Web.Mvc;

namespace BankingWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View();
        }
      
    }
}
