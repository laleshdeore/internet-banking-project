using System.Web.Mvc;
using BankingDAL.Repository;
using BankingWeb.Models;

namespace BankingWeb.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IUserRepository _userRepository;

        public ServiceController()
        {
            _paymentRepository = new PaymentRepository(Context);
            _regionRepository = new RegionRepository(Context);
            _userRepository = new UserRepository(Context);
        }

        [Authorize]
        public ActionResult All()
        {
            return View(_paymentRepository.GetServices());
        }

        [Authorize]
        public ActionResult Add()
        {
            return View(new ServiceModel { Regions = _regionRepository.GetRegions() });
        }

        [Authorize]
        [HttpPost]
        public ActionResult Add(ServiceModel serviceModel)
        {
            _paymentRepository.AddOrUpdate(serviceModel.GetEntity(_regionRepository, _userRepository));

            return RedirectToAction("All", "Service");
        }

        [Authorize]
        public ActionResult Delete(long id)
        {
            _paymentRepository.GetServiceById(id);

            return RedirectToAction("All", "Service");
        }
    }
}