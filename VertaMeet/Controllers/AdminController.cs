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
                Users = DatabaseInteractor.GetAllUsers(),
                InterestGroups = DatabaseInteractor.GetAllInterestGroups(),
            };
            model.Events = GetEventsForInterestGroups(model.InterestGroups);

            return View(model);
        }

        private List<EventModel> GetEventsForInterestGroups(List<InterestGroupModel> interestGroups)
        {
            List<EventModel> output = new List<EventModel>();

            foreach (var interestGroup in interestGroups)
            {
                output.AddRange(DatabaseInteractor.GetEventsForInterestGroup(interestGroup.Id));
            }

            return output;
        }
    }
}