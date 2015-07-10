using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VertaMeet.Models
{
    public class EventModel : IDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public string ImageUrl { get; set; }
        public string Location { get; set; }
        public InterestGroupModel InterestGroup { get; set; }
        public List<UserModel> Attendees { get; set; }
    }
}