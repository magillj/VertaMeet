using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VertaMeet.Models;
using VertaMeet.Data;

namespace VertaMeet.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetUserById()
        {
            DatabaseInteractor.GetUserById(1);

            return View();
        }
    }
}