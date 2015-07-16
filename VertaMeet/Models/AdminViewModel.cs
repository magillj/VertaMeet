using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VertaMeet.Models
{
    public class AdminViewModel: IViewModel
    {
        public List<UserModel> Users { get; set; }
        
        public List<InterestGroupModel> InterestGroups { get; set; }

        public List<EventModel> Events { get; set; } 
    }
}