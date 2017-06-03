using EduSpot.Entity.Tables.Organization;
using Esdm.Repository.Abstraction.Entity.Organization;
using Esdm.Repository.Concrete.Entity.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Esdm.Web.Areas.Organization.Controllers
{
    public class DirectorsController : Controller
    {
        private IManagementRepository repo = new ManagementRepository();

        public JsonResult LoadDirectors(string id)
        {
            var data = repo.FindByCompany(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<string> CreateService(Management model)
        {
            if (ModelState.IsValid)
            {
                model.ID = Guid.NewGuid().ToString();
                model.CreatedBy = User.Identity.Name;
                model.CreatedDate = DateTime.Now;

                await repo.AddAsync(model);
                return model.ID;
            }
            return "0";
        }

        public async Task<JsonResult> FindById(string id)
        {
            var data = await repo.FindAsync(id);
            var result = new
            {
                ID = data.ID,
                Status = data.Status,
                CompanyID = data.CompanyID,
                Name = data.Name,
                NPWP = data.NPWP,
                Country = data.Country,
                AdditionalInformation = data.AdditionalInformation
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<string> EditService(Management model)
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
        public async Task<string> DeleteService(string id)
        {
            var model = await repo.FindAsync(id);
            await repo.RemoveAsync(model);
            return "OK";
        }
    }
}