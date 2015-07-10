using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VertaMeet.Models
{
    public class InterestGroupModel
    {
        public string Name;

        public int Id;

        public UserModel Manager;

        public List<UserModel> Members;

        public string ImageUrl;
    }
}