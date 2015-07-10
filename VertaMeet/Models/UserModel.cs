using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VertaMeet.Models
{
    public class UserModel : IDataModel
    {
        public enum LOCATION
        {
            Bothell,
            Atlanta
        };

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public LOCATION Location { get; set; }
    }

    //public class UserLocation
    //{
    //    private UserLocation(string value)

    //    public string Value { get; }

    //    public static 
    //}
}