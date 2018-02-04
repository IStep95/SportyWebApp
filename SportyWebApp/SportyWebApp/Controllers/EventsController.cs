using SportyWebApp.Models;
using SportyWebApp.WebAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

namespace SportyWebApp.Controllers
{
    public class EventsController : Controller
    {

        API api = new API();
        List<SportViewModel> allSports = new List<SportViewModel>();
        List<EventListModel> searchEvents = new List<EventListModel>();

        [HttpGet]
        public ActionResult Subscribe(int id)
        {
            return PartialView("_Subscribe", id);
        }
        // GET: Search
        public async Task<ActionResult> Search(string sportId, string date, string cityName, string freePlayers)
        {                      
            UserViewModel userViewModel = (UserViewModel) Session["UserViewModel"];
            if (userViewModel == null) return RedirectToAction("Login", "User");

            allSports = await api.HttpGetAllSports(); 

            // First request
            if (sportId == null)
            {
                ViewBag.CityEntered = true;
                ViewBag.AllSports = allSports;
                ViewBag.SearchEvents = searchEvents;
                ViewBag.CurrentPage = "SearchEventPage";
                return View();
            }
            
            // Search request
            if (cityName == "")
            {
                ViewBag.CityEntered = false;
                ViewBag.AllSports = allSports;
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
            }

            int resultsDivIDHeight = searchEvents.Count * 200;
            if (resultsDivIDHeight < 400) resultsDivIDHeight = 500;
            int findMaindDivHeight = 250 + resultsDivIDHeight;


            ViewBag.CurrentPage = "SearchEventPage";
            ViewBag.AllSports = allSports;
            ViewBag.SearchEvents = searchEvents;
            ViewBag.CityName = cityName;
            return View();
        }
        
        public async Task<ActionResult> FutureEvents()
        {
            UserViewModel userViewModel = (UserViewModel)Session["UserViewModel"];
            if (userViewModel == null) return RedirectToAction("Login", "User");

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
            UserViewModel userViewModel = (UserViewModel)Session["UserViewModel"];
            if (userViewModel == null) return RedirectToAction("Login", "User");

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
            UserViewModel userViewModel = (UserViewModel)Session["UserViewModel"];
            if (userViewModel == null) return RedirectToAction("Login", "User");

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
            UserViewModel userViewModel = (UserViewModel)Session["UserViewModel"];
            if (userViewModel == null) return RedirectToAction("Login", "User");

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
            UserViewModel userViewModel = (UserViewModel)Session["UserViewModel"];
            if (userViewModel == null) return RedirectToAction("Login", "User");

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
        public class RequireRequestValueAttribute : ActionMethodSelectorAttribute
        {
            public RequireRequestValueAttribute(string valueName)
            {
                if (valueName == null)
                {
                    ValueName = "";
                }
                else
                {
                    ValueName = valueName;
                }         
            }
            public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
            {
                return (controllerContext.HttpContext.Request[ValueName] != null);
            }

            public string ValueName { get; private set; }
        }

    }

}

