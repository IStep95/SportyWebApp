using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportyWebApp.Models
{
    public class EventListModel
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        [Display(Name = "Sport")]
        public string SportName { get; set; }
        [Display(Name = "Lokacija")]
        public string Location { get; set; }
        [Display(Name = "Grad")]
        public string CityName { get; set; }
        public int MaxPlayers { get; set; }
        public int FreePlayers { get; set; }
        [Display(Name = "Vrijeme početka")]
        public DateTime StartTime { get; set; }
    }
}