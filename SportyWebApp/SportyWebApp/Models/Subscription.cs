using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportyWebApp.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int SportId { get; set; }
        public string CityName { get; set; }
    }
}