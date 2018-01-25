using SportyWebApp.Models;
using SportyWebApp.WebAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SportyWebApp.Controllers
{
    public class EventsController : Controller
    {

        API api = new API();
        List<SportViewModel> allSports = new List<SportViewModel>();
        List<EventViewModel> searchEvents = new List<EventViewModel>();

        // GET: Search
        public async Task<ActionResult> Search(string sportId, string date, string cityName, string freePlayers)
        {
                       
            UserViewModel userViewModel = (UserViewModel) Session["UserViewModel"];
            if (userViewModel == null) return RedirectToAction("Login", "User");

            allSports = await api.HttpGetAllSports(); 

            // First call
            if (sportId == null)
            {
                ViewBag.CityEntered = true;
                ViewBag.AllSports = allSports;
                ViewBag.FindMainDivHeight = "height: " + 900 + "px";
                ViewBag.SearchEvents = searchEvents;
                ViewBag.CurrentPage = "SearchEventPage";
                return View();
            }

            // TODO: Search events 
            if (cityName == "")
            {
                ViewBag.CityEntered = false;
                ViewBag.AllSports = allSports;
                ViewBag.FindMainDivHeight = "height: " + 900 + "px";
                ViewBag.SearchEvents = searchEvents;
                ViewBag.CurrentPage = "SearchEventPage";
                return View();
            }

            ViewBag.CityEntered = true;
            searchEvents = await api.HttpFindEvents(sportId, date, cityName, freePlayers);
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

            int resultsDivIDHeight = searchEvents.Count * 200;
            if (resultsDivIDHeight < 400) resultsDivIDHeight = 500;
            int findMaindDivHeight = 250 + resultsDivIDHeight;

            ViewBag.FindMainDivHeight = "height: " + findMaindDivHeight + "px";
            ViewBag.SideBarWrapperHeight = "height: " + findMaindDivHeight + "px";
            ViewBag.CurrentPage = "SearchEventPage";
            ViewBag.AllSports = allSports;
            ViewBag.SearchEvents = searchEvents;
            ViewBag.CityName = cityName;
            return View();
        }

        public async Task<ActionResult> FutureEvents()
        {
            string username = ((UserViewModel)Session["UserViewModel"]).UserName;
            string url = HttpContext.Request.Url.AbsoluteUri;
            int index = url.LastIndexOf('/');
            string time = url.Substring(index+1);

            List<EventListModel> events = await api.HttpGetEvents(username, time.ToLower());
            ViewBag.FutureEvents = events;
            return View((UserViewModel)Session["UserViewModel"]);
        }

        public async Task<ActionResult> PastEvents()
        {
            string username = ((UserViewModel)Session["UserViewModel"]).UserName;
            string url = HttpContext.Request.Url.AbsoluteUri;
            int index = url.LastIndexOf('/');
            string time = url.Substring(index + 1);

            List<EventListModel> events = await api.HttpGetEvents(username, time.ToLower());
            ViewBag.PastEvents = events;
            return View((UserViewModel)Session["UserViewModel"]);
        }

        // GET: Event/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Event/Create
        public ActionResult Create()
        {
            List<SportViewModel> lst = new List<SportViewModel>()
            {
                new SportViewModel() {Id=1, Name="Mali nogomet"},
                new SportViewModel() {Id=2, Name="Košarka"},
                new SportViewModel() {Id=3, Name="Odbojka"}
            };
            EventCreateModel model = new EventCreateModel();
            model.lstSports = lst;
            return View(model);
        }

        // POST: Event/Create
        [HttpPost]
        public async Task<ActionResult> Create(EventCreateModel model)
        {
            int hours = -1, minutes = -1;
            string[] time = model.Time.Split(':');
            if (!time[0].Equals("0") && !time[0].Equals("00"))
            {
                Int32.TryParse(time[0], out hours);
            }
            if (!time[1].Equals("0") && !time[1].Equals("00"))
            {
                Int32.TryParse(time[1], out minutes);
            }
            if (minutes == 0 || hours == 0)
                return RedirectToAction("Create");
            if (hours == -1)
                hours = 0;
            if (minutes == -1)
                minutes = 0;
            TimeSpan ts = new TimeSpan(hours, minutes, 0);
            model.Date = model.Date.Date + ts;
            model.UserName = ((UserViewModel)Session["UserViewModel"]).UserName;
            try
            {
                string response = await api.HttpCreateEvent(model);
                if (response.Equals("OK"))
                {
                    return RedirectToAction("FutureEvents");
                }
                else
                {
                    ViewBag.poruka = response;
                    return RedirectToAction("Create");
                }
            }
            catch
            {
                return RedirectToAction("Create");
            }
        }

        // GET: Event/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Event/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Event/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }

}

