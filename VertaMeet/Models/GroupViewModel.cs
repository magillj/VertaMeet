using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VertaMeet.Models
{
    public class GroupViewModel : IViewModel
    {
        public InterestGroupModel InterestGroup { get; set; }
    }
}