using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportyWebApp.Models
{
    public class UserEventModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public decimal Rating { get; set; }
    }
}