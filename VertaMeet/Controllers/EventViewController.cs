using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VertaMeet.Data;
using VertaMeet.Models;

namespace VertaMeet.Controllers
{
    public class EventViewController : Controller
    {
        // GET: EventView
        public ActionResult Index(int eventId)
        {
            return View(new EventViewModel()
            {
                Event = DatabaseInteractor.GetEventById(eventId)
            });
        }
    }
}