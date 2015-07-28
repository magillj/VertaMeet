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
    public class InterestGroupController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage CreateInterestGroup(InterestGroupFormData formData)
        {
            return CreateFullInterestGroup(new InterestGroupModel()
                {
                    Description = formData.Description,
                    Id = 1,
                    ImageUrl = formData.ImageUrl,
                    Manager = DatabaseInteractor.GetUserById(formData.ManagerId),
                    Members = new List<UserModel>(),
                    Name = formData.Name
                });
        }

        [HttpPost]
        public HttpResponseMessage CreateFullInterestGroup(InterestGroupModel interestGroupModel)
        {
            if (interestGroupModel.Name == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Interest group could not be created, missing name");
            }

            interestGroupModel.Id = DatabaseInteractor.GetHighestInterestGroupId() + 1;

            // Add manager to members
            if (interestGroupModel.Members.Count == 0)
            {
                interestGroupModel.Members.Add(interestGroupModel.Manager);
            }

            DatabaseInteractionResponse result = DatabaseInteractor.CreateInterestGroup(interestGroupModel);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK, interestGroupModel);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "Interest group could not be created. The following error occurred: " + result.Message);
        }

        [HttpPost]
        public HttpResponseMessage AddUserToInterestGroup(InterestGroupUserIds ids)
        {
            DatabaseInteractionResponse result = DatabaseInteractor.AddUserToInterestGroup(ids.InterestGroupId, ids.UserId);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "User could not be added to interest group. The following error occurred: " + result.Message);
        }

        [HttpPost]
        public HttpResponseMessage RemoveUserFromInterestGroup(InterestGroupUserIds ids)
        {
            DatabaseInteractionResponse result = DatabaseInteractor.RemoveUserFromInterestGroup(ids.InterestGroupId, ids.UserId);

            if (result.Success)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest, "User could not be removed from interest group. The following error occurred: " + result.Message);
        }
    }

    // Used to accept special requests
    public class InterestGroupUserIds
    {
        public int InterestGroupId { get; set; }
        public int UserId { get; set; }
    }

    public class InterestGroupFormData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl {get; set; }
        public int ManagerId { get; set; }
    }
}