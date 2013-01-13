using System.Web.Mvc;
using BankingDAL.Repository;
using BankingWeb.Models;

namespace BankingWeb.Controllers
{
    public class ServiceController : BaseController
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IAccountRepository _accountRepository;

        public ServiceController()
        {
            _paymentRepository = new PaymentRepository(Context);
            _regionRepository = new RegionRepository(Context);
            _accountRepository = new AccountRepository(Context);
        }

        [Authorize]
        public ActionResult All()
        {
            return View(User.IsInRole(Administrator) || User.IsInRole(Employee) ? _paymentRepository.GetServices() : CurrentUser.Region.Services);
        }

        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Add()
        {
            return View(new ServiceModel { Regions = _regionRepository.GetRegions() });
        }

        [Authorize(Roles = AdminOrEmployee)]
        [HttpPost]
        public ActionResult Add(ServiceModel serviceModel)
        {
            var service = serviceModel.GetEntity(_regionRepository, _accountRepository);

            if (service.Account == null)
            {
                ModelState.AddModelError("account", "Account does not exist");
            }

            if (ModelState.IsValid)
            {
                _paymentRepository.AddOrUpdate(service);
            }

            if (!ModelState.IsValid)
            {
                serviceModel.Regions = _regionRepository.GetRegions();
                return View(serviceModel);
            }
            return RedirectToAction("All", "Service");
        }

        [Authorize(Roles = AdminOrEmployee)]
        public ActionResult Delete(long id)
        {
            _paymentRepository.Delete(_paymentRepository.GetServiceById(id));

            return RedirectToAction("All", "Service");
        }
    }
}