using System.Web.Mvc;
using BankingDAL.Repository;

namespace BankingWeb.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IPaymentRepository _paymentRepository;

        public ServiceController()
        {
            _paymentRepository = new PaymentRepository(Context);
        }

        [Authorize]
        public ActionResult All()
        {
            return View(_paymentRepository.GetServices());
        }
    }
}