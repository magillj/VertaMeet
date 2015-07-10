using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VertaMeet.Models;

namespace VertaMeet.Domain.Tier
{
    public class EventService
    {
        public static List<EventModel> Events = new List<EventModel>();

        static EventService()
        {
            #region Old Dummy Data
            /*  
            //Outdoors Group
            Events.Add(new EventModel() { Name = "Twin Falls Hike", Description = "We'll be hiking an easy, local trail called Twin Falls. Great views, great people. Meet at the hike location. Let us know if you can drive.", Attendees = UserService.GetRandomUsers(15), Start = DateTime.Now.AddHours(2), End = DateTime.Now.AddHours(4), Host = UserService.GetRandomUser(), Location = "Twin Falls, WA", InterestGroup =  });

            Events.Add(new EventModel() { Name = "Mount Dickerman Hike", Description = "With a 4000 ft vertical climb and an 8.6 mile roundtrip, this hike is only for those in good shape and who want a challenge. Let us know if you can drive.", Attendees = UserService.GetRandomUsers(8), Start = DateTime.Now.AddHours(24), End = DateTime.Now.AddHours(28), Host = UserService.GetRandomUser(), Location = "Twin Falls, WA" });

            Events.Add(new EventModel() { Name = "Twin Falls Hike", Description = "We'll be hiking an easy, local trail called Twin Falls. Great views, great people. Meet at the hike location. Let us know if you can drive.", Attendees = UserService.GetRandomUsers(10), Start = DateTime.Now.AddHours(2), End = DateTime.Now.AddHours(4), Host = UserService.GetRandomUser(), Location = "Twin Falls, WA" });
            */
            #endregion


        }
    }
}