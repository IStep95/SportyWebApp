using Newtonsoft.Json.Linq;
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
    public class EventController : Controller
    {
        // GET: Event
        //public async Task<ActionResult> MyEvents()
        //{
        //    //string username = Session["Username"].ToString();
        //    //List<EventViewModel> events = await api.HttpGetEvents(username);
        //    //return View(events);
        //}

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
            API api = new API();
            int hours=-1, minutes=-1;
            string[] time = model.Time.Split(':');
            if(!time[0].Equals("0") && !time[0].Equals("00"))
            {
                Int32.TryParse(time[0], out hours);
            }
            if (!time[1].Equals("0") && !time[1].Equals("00"))
            {
                Int32.TryParse(time[1], out minutes);
            }
            if(minutes == 0 || hours == 0)
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
                    return RedirectToAction("MyEvents");
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
