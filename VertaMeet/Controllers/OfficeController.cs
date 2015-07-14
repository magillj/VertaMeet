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
        // GET: Office
        public ActionResult Index(string location)
        {
            UserModel.LOCATION loc;
            if (!Enum.TryParse(location, true, out loc))
            {
                return new HttpStatusCodeResult(400, "Location is invalid");
            }

            return View(new OfficeViewModel()
            {
                InterestGroups = DatabaseInteractor.GetAllInterestGroups(),
                Location = loc
            });
        }
    }
}