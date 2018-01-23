using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportyWebApp.Models
{
    public class EventCreateModel
    {
        public List<SportViewModel> lstSports { get; set; }
        public int SportId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        public DateTime Date { get; set; }
        [Display(Name = "Vrijeme")]
        public string Time { get; set; }
        [Display(Name = "Grad")]
        public string City { get; set; }
        [Display(Name = "Lokacija")]
        public string Location { get; set; }
        [Display(Name = "Ukupan broj igrača")]
        public int MaxPlayers { get; set; }
        [Display(Name = "Broj potrebnih igrača")]
        public int FreePlayers { get; set; }
        public string UserName { get; set; }
    }
}