using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportyWebApp.Models
{
    public class EventDetailsModel : EventListModel
    {
        public List<UserEventModel> lstUsers { get; set; }
    }
}