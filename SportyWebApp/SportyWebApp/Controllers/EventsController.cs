using SportyWebApp.Models;
using SportyWebApp.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SportyWebApp.Controllers
{
    public class EventsController : Controller
    {
        API _api = new API();

        // GET: Search
        public async Task<ActionResult> Search(string sportId, string date, string cityName, string freePlayers)
        {
            _api = new API();

            UserViewModel uvm = (UserViewModel) Session["UserViewModel"];

            // TODO: Search events 
            List<EventViewModel> searchEvents = await _api.HttpGetTodayEvents(uvm.UserName);
            foreach (var entry in searchEvents)
            {
                var sportName = entry.SportName;
                if (!String.IsNullOrEmpty(sportName))
                {
                    sportName = sportName.First().ToString().ToUpper() + sportName.Substring(1);
                    entry.SportName = sportName;
                }
                if (cityName != null)
                {
                    entry.City = new CityViewModel();
                    entry.City.Name = cityName;
                }
            }

            List<SportViewModel> allSports = await _api.HttpGetAllSports();
            
           
            ViewBag.MainTitle = "Traži događaj";
            ViewBag.CurrentPage = "SearchEventPage";
            ViewBag.AllSports = allSports;
            ViewBag.SearchEvents = searchEvents;
            ViewBag.CityName = cityName;
            return View();
        }

    }
}
