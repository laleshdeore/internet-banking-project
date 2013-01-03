using System.Web.Mvc;
using BankingDAL.Repository;
using BankingWeb.Models;

namespace BankingWeb.Controllers
{
    public class RegionController : BaseController
    {
        private readonly IRegionRepository _regionRepository;

        public RegionController()
        {
            _regionRepository = new RegionRepository(Context);
        }

        public ActionResult All()
        {
            return View(_regionRepository.GetRegions());
        }

        public ActionResult Delete(long id)
        {
            _regionRepository.Delete(_regionRepository.GetRegionById(id));
            return RedirectToAction("All", "Region");
        }

        [HttpPost]
        public ActionResult Add(RegionModel regionModel)
        {
            _regionRepository.AddOrUpdate(regionModel.GetEntity());
            return RedirectToAction("All", "Region");
        }

        public ActionResult Add()
        {
            return View(new RegionModel());
        }
    }
}