using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VertaMeet.Models
{
    public class OfficeViewModel : IViewModel
    {
        public UserModel.LOCATION Location { get; set; }

        public List<InterestGroupModel> InterestGroups { get; set; }
    }
}