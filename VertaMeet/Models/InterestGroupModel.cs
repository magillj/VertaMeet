using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VertaMeet.Models
{
    public class InterestGroupModel : IDataModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }
        public UserModel Manager { get; set; }
        public List<UserModel> Members { get; set; }
        public string ImageUrl { get; set; }
    }
}