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
            LoadState();
            return View(_regionRepository.GetRegions());
        }

        public ActionResult Delete(long id)
        {
            var region = _regionRepository.GetRegionById(id);

            if (region.Services.Count > 0)
            {
                ModelState.AddModelError("services", "Some services assigned to this region");
            }

            if (region.Users.Count > 0)
            {
                ModelState.AddModelError("users", "Some users assigned to this region");
            }

            if (ModelState.IsValid)
            {
                _regionRepository.Delete(_regionRepository.GetRegionById(id));
            }
            SaveState();
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