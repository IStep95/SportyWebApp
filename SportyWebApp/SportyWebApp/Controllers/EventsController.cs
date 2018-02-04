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

        API api = new API();

        // GET: Search
        public async Task<ActionResult> Search(string sportId, string date, string cityName, string freePlayers)
        {

            UserViewModel uvm = (UserViewModel) Session["UserViewModel"];

            // TODO: Search events 
            List<EventViewModel> searchEvents = await api.HttpGetTodayEvents(uvm.UserName);
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

            List<SportViewModel> allSports = await api.HttpGetAllSports();
            
           
            ViewBag.MainTitle = "Traži događaj";
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
            ViewBag.MainTitle = "Moji događaji";
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
            ViewBag.MainTitle = "Moji događaji";
            return View((UserViewModel)Session["UserViewModel"]);
        }

        // GET: Event/Details/5
        public async Task<ActionResult> Details(int id)
        {
            EventDetailsModel model = await api.HttpGetEvent(id);
            bool isPlayer = false;
            string username = ((UserViewModel)Session["UserViewModel"]).UserName;
            int count = model.lstUsers.Where(e => e.UserName == username).Count();
            if (count > 0)
                isPlayer = true;
            ViewBag.isPlayer = isPlayer;
            ViewBag.users = model.lstUsers;
            ViewBag.MainTitle = "Pregled događaja";
            return View(model);
        }

        // GET: Event/Create
        public async Task<ActionResult> Create()
        {
            List<SportViewModel> lst = await api.HttpGetAllSports();
            EventCreateModel model = new EventCreateModel();
            model.lstSports = lst;
            ViewBag.MainTitle = "Novi dagađaj";
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
            {
                List<SportViewModel> lst = await api.HttpGetAllSports();
                model.lstSports = lst;
                ViewBag.MainTitle = "Novi dagađaj";
                ViewBag.Message = "Unesite ispravno vrijeme";
                return View(model);
            }
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
                    List<SportViewModel> lst = await api.HttpGetAllSports();
                    model.lstSports = lst;
                    ViewBag.MainTitle = "Novi dagađaj";
                    ViewBag.Message = response;
                    return View(model);
                }
            }
            catch
            {
                List<SportViewModel> lst = await api.HttpGetAllSports();
                ViewBag.MainTitle = "Novi dagađaj";
                model.lstSports = lst;
                ViewBag.Message = "Nemoguće izvršiti akciju";
                return View(model);
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

