using SportyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportyWebApp.Controllers
{
    public class EventsController : Controller
    {
        // GET: Search
        public ActionResult Search(string sport, string date, string city, string freePlayers)
        {
            //List<EventViewModel> todayEvents = await _api.HttpGetTodayEventsByCityId(_userViewModel.CityId);
            //foreach (var entry in todayEvents)
            //{
            //    var sportName = entry.SportName;
            //    if (!String.IsNullOrEmpty(sportName))
            //    {
            //        sportName = sportName.First().ToString().ToUpper() + sportName.Substring(1);
            //        entry.SportName = sportName;
            //    }
            //}

            ViewBag.CurrentPage = "SearchEventPage";
            return View();
        }

    }
}
