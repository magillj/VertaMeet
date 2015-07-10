using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VertaMeet.Models
{
    /// <summary>
    /// A model class speicifcally for storing information derived from the database
    /// </summary>
    interface IDataModel
    {
        int Id { get; set; }
    }
}
