using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VertaMeet.Data;
using VertaMeet.Models;

namespace VertaMeet.Controllers
{
    public class GroupController : Controller
    {
        // GET: Group
        public ActionResult Index(int id)
        {
            return View(new GroupViewModel()
            {
                InterestGroup = DatabaseInteractor.GetInterestGroupById(id)
            });
        }
    }
}