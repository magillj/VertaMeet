﻿using System;
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
    [RoutePrefix("api/event")]
    public class EventController : ApiController
    {
        /* Autogenerated Code 

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Event
        public IQueryable<EventModel> GetEventModels()
        {
            return db.EventModels;
        }

        // GET: api/Event/5
        [ResponseType(typeof(EventModel))]
        public IHttpActionResult GetEventModel(int id)
        {
            EventModel eventModel = db.EventModels.Find(id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return Ok(eventModel);
        }

        // PUT: api/Event/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEventModel(int id, EventModel eventModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventModel.Id)
            {
                return BadRequest();
            }

            db.Entry(eventModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Event
        [ResponseType(typeof(EventModel))]
        public IHttpActionResult PostEventModel(EventModel eventModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventModels.Add(eventModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = eventModel.Id }, eventModel);
        }

        // DELETE: api/Event/5
        [ResponseType(typeof(EventModel))]
        public IHttpActionResult DeleteEventModel(int id)
        {
            EventModel eventModel = db.EventModels.Find(id);
            if (eventModel == null)
            {
                return NotFound();
            }

            db.EventModels.Remove(eventModel);
            db.SaveChanges();

            return Ok(eventModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventModelExists(int id)
        {
            return db.EventModels.Count(e => e.Id == id) > 0;
        }

        */

        [Route("CreateEvent")]
        [HttpPost]
        public HttpResponseMessage CreateEventModel(EventModel eventModel)
        {
            if (eventModel.Name == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Event could not be created, missing name");
            }

            DatabaseInteractionResponse result = DatabaseInteractor.CreateEvent(eventModel);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Event could not be created. The following error occurred: " + result.Message);
        }

        [Route("AddUserToEvent")]
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

        [Route("RemoveUserFromEvent")]
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
}