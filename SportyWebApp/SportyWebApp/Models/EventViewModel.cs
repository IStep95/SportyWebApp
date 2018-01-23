using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportyWebApp.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public int MaxPlayers { get; set; }
        public int FreePlayers { get; set; }
        public DateTime StartTime { get; set; }
        public string Location { get; set; }
        public CityViewModel City { get; set; }
        public int SportId { get; set; }
        public string SportName { get; set; }
        public UserViewModel Creator { get; set; }
        public List<UserViewModel> Participants {get; set;}

    }
}