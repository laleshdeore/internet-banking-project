using System.Web.Mvc;

namespace BankingWeb.Controllers
{
    public class PaymentController : Controller
    {
        public ActionResult Pay()
        {
            return View();
        }
    }
}