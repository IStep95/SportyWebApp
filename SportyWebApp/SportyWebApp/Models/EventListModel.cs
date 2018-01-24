using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportyWebApp.Models
{
    public class EventListModel
    {
        public int SportId { get; set; }
        public string SportName { get; set; }
        public string Location { get; set; }
        public string CityName { get; set; }
        public int MaxPlayers { get; set; }
        public int FreePlayers { get; set; }
        public DateTime StartTime { get; set; }
    }
}