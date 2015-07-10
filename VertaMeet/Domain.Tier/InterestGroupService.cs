using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VertaMeet.Models;

namespace VertaMeet.Domain.Tier
{
    public class InterestGroupService
    {
        public static List<InterestGroupModel> InterestGroups;

        static InterestGroupService()
        {
            //InterestGroups = new List<InterestGroupModel>();
            //InterestGroups.Add(new InterestGroupModel() { Name = "Outdoor Activities", ID = 1, Manager = UserService.GetRandomUser(), Members = UserService.GetRandomUsers(50), ImageURL = "http://wilderness.org/sites/default/files/styles/blog_full/public/boots%20at%20lake%20photo.jpg?itok=nIFuJ6G5" });
            //InterestGroups.Add(new InterestGroupModel() { Name = "Thai Cuisine Afficionados", ID = 2, Manager = UserService.GetRandomUser(), Members = UserService.GetRandomUsers(20), ImageURL = "https://upload.wikimedia.org/wikipedia/commons/1/18/Lanna_cuisine_starters.JPG" });
        }
    }
}