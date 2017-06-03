using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class RKABController : Controller
    {
        private IRKABRepository rkabRepository = new RKABRepository();

        public async Task<JsonResult> FindById(string id)
        {
            var data = await rkabRepository.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Status = data.Status,
                CompanyID = data.CompanyID,
                RkabYear = data.RkabYear,
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditService(RKAB model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = User.Identity.Name;
                model.ModifiedDate = DateTime.Now;
                await rkabRepository.UpdateAsync(model);
                return model.ID;
            }
            return "0";
        }

        [HttpPost]
        public async Task<string> CreateService(RKAB model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Guid.NewGuid().ToString();
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;

                var result = await rkabRepository.AddAsync(model);
                return result.ID;

            }
            return "-1";
        }

        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            RKAB model = await rkabRepository.FindAsync(id);
            await rkabRepository.RemoveAsync(model);
            return "OK";
        }

    }
}