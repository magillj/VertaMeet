using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VertaMeet.Data;
using VertaMeet.Models;

namespace VertaMeet.Controllers
{
    public class EventController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage CreateEvent(EventFormData formData)
        {
            return CreateFullEvent(new EventModel()
            {
                Attendees = new List<UserModel>(),
                Description = formData.Description,
                Id = -1,
                ImageUrl = formData.ImageUrl,
                InterestGroup = DatabaseInteractor.GetInterestGroupById(formData.InterestGroupId),
                Location = formData.Location,
                Name = formData.Name,
                Time = DateTime.Parse(formData.Time)
            });
        }

        [HttpPost]
        public HttpResponseMessage CreateFullEvent(EventModel eventModel)
        {
            if (eventModel.Name == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Event could not be created, missing name");
            }

            eventModel.Id = DatabaseInteractor.GetHighestEventId() + 1;

            DatabaseInteractionResponse result = DatabaseInteractor.CreateEvent(eventModel);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, eventModel);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Event could not be created. The following error occurred: " + result.Message);
        }

        [HttpPost]
        public HttpResponseMessage AddUserToEvent(EventUserIds ids)
        {
            DatabaseInteractionResponse result = DatabaseInteractor.AddUserToEvent(ids.EventId, ids.UserId);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "User could not be added to event. The following error occurred: " + result.Message);
        }

        [HttpPost]
        public HttpResponseMessage RemoveUserFromEvent(EventUserIds ids)
        {
            DatabaseInteractionResponse result = DatabaseInteractor.RemoveUserFromEvent(ids.EventId, ids.UserId);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "User could not be removed from event. The following error occurred: " + result.Message);
        }
    }

    // Used to accept special requests
    public class EventUserIds
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
    }

    public class EventFormData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
        public string ImageUrl { get; set; }
        public string Location { get; set; }
        public int InterestGroupId { get; set; }
    }
}