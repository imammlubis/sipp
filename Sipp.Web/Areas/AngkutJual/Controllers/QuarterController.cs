using EduSpot.Entity.Tables.AngkutJual;
using Esdm.Repository.Abstraction.Entity.AngkutJual;
using Esdm.Repository.Concrete.Entity.AngkutJual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.AngkutJual.Controllers
{
    public class QuarterController : Controller
    {
        private IQuarterlyRepository repo = new QuarterlyRepository();

        public async Task<JsonResult> FindById(string id)
        {
            var data = await repo.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Status = data.Status,
                CompanyID = data.CompanyID,
                Year = data.Year,
                Period = data.Period
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditService(Quarterly model)
        {
            if (ModelState.IsValid)
            {
                model.ModifiedBy = User.Identity.Name;
                model.ModifiedDate = DateTime.Now;
                await repo.UpdateAsync(model);
                return model.ID;
            }
            return "0";
        }

        [HttpPost]
        public async Task<string> CreateService(Quarterly model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Guid.NewGuid().ToString();
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;

                var result = await repo.AddAsync(model);
                return result.ID;

            }
            return "-1";
        }

        [HttpPost]
        public async Task<string> DeleteService(string id)
        {
            Quarterly model = await repo.FindAsync(id);
            await repo.RemoveAsync(model);
            return "OK";
        }
    }
}