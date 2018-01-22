using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportyWebApp.Models
{
    public class RatingViewModel
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public SportViewModel Sport { get; set; }
    }
}