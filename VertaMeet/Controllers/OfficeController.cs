using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VertaMeet.Data;
using VertaMeet.Models;

namespace VertaMeet.Controllers
{
    public class OfficeController : Controller
    {
        public ActionResult Index(string id)
        {
            UserModel.LOCATION location;
            if (!Enum.TryParse(id, true, out location))
            {
                return new HttpStatusCodeResult(400, "Location is invalid");
            }
            OfficeViewModel model = new OfficeViewModel()
            {
                InterestGroups = DatabaseInteractor.GetAllInterestGroups(),
                Location = location
            };

            return View(model);
        }
    }
}