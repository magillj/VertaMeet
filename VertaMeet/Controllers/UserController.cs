﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net.Http.Formatting;
using VertaMeet.Models;
using VertaMeet.Data;

namespace VertaMeet.Controllers
{
    public class UserController : ApiController
    {
        #region Autogenerated Code
        /*
        private ApplicationDbContext db = new ApplicationDbContext(); 

        // GET: api/User
        public IQueryable<UserModel> GetUserModels()
        {
            return db.UserModels; 
        }

        // GET: api/User/5
        [ResponseType(typeof(UserModel))]
        public IHttpActionResult GetUserModel(int id)
        {
            UserModel userModel = db.UserModels.Find(id);
            if (userModel == null)
            {
                return NotFound();
            }

            return Ok(userModel);
        }

        // PUT: api/User/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserModel(int id, UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userModel.Id)
            {
                return BadRequest();
            }

            db.Entry(userModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
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

        // POST: api/User
        [ResponseType(typeof(UserModel))]
        public IHttpActionResult CreateUser(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserModels.Add(userModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userModel.Id }, userModel);
        }

        // DELETE: api/User/5
        [ResponseType(typeof(UserModel))]
        public IHttpActionResult DeleteUserModel(int id)
        {
            UserModel userModel = db.UserModels.Find(id);
            if (userModel == null)
            {
                return NotFound();
            }

            db.UserModels.Remove(userModel);
            db.SaveChanges();

            return Ok(userModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserModelExists(int id)
        {
            return db.UserModels.Count(e => e.Id == id) > 0;
        }
        */
        #endregion

        // GET: api/User/5
        public IHttpActionResult GetUserModel(int id)
        {
            UserModel userModel = DatabaseInteractor.GetUserById(id);

            if (userModel == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPost]
        public HttpResponseMessage CreateUser(UserModel user)
        {
            if (user.Name == null || user.Email == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User could not be created, missing name and/or email.");
            }

            // Set unique user id
            user.Id = DatabaseInteractor.GetHighestUserId() + 1;

            DatabaseInteractionResponse result = DatabaseInteractor.CreateUser(user);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            
            return Request.CreateResponse(HttpStatusCode.BadRequest, "User could not be created. The following error occurred: " + result.Message);
        }

        [HttpPost]
        public HttpResponseMessage DeleteUser(int userId)
        {
            DatabaseInteractionResponse result = DatabaseInteractor.DeleteUser(userId);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "User could not be created. The following error occurred: " + result.Message);
        }
    }
}