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
            //UserEventModel user1 = new UserEventModel();
            //user1.FirstName = "Ime";
            //user1.LastName = "Prezime";
            //user1.UserName = "Korisničko ime";
            //user1.Rating = 3;
            //UserEventModel user2 = new UserEventModel();
            //user2.FirstName = "Ime";
            //user2.LastName = "Prezime";
            //user2.UserName = "Korisničko ime";
            //user2.Rating = 3;
            //UserEventModel user3 = new UserEventModel();
            //user3.FirstName = "Ime";
            //user3.LastName = "Prezime";
            //user3.UserName = "Korisničko ime";
            //user3.Rating = 3;
            //UserEventModel user4 = new UserEventModel();
            //user4.FirstName = "Ime";
            //user4.LastName = "Prezime";
            //user4.UserName = "Korisničko ime";
            //user4.Rating = 3;
            //UserEventModel user5 = new UserEventModel();
            //user5.FirstName = "Ime";
            //user5.LastName = "Prezime";
            //user5.UserName = "Korisničko ime";
            //user5.Rating = 3;
            //UserEventModel user6 = new UserEventModel();
            //user6.FirstName = "Ime";
            //user6.LastName = "Prezime";
            //user6.UserName = "Korisničko ime";
            //user6.Rating = 3;
            //model.lstUsers.Add(user1);
            //model.lstUsers.Add(user2);
            //model.lstUsers.Add(user3);
            //model.lstUsers.Add(user4);
            //model.lstUsers.Add(user5);
            //model.lstUsers.Add(user6);
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

