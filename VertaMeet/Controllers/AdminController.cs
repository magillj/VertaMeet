using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VertaMeet.Models;
using VertaMeet.Data;

namespace VertaMeet.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            AdminViewModel model = new AdminViewModel()
            {
                Users = DatabaseInteractor.GetAllUsers()
            };

            return View(model);
        }
    }
}