using SportyWebApp.Models;
using SportyWebApp.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportyWebApp.Controllers
{
    public class SubscriptionController : Controller
    {
        API api = new API();
        [HttpPost]
        public async Task<ActionResult> GetSubscriptionEvents(int SubId)
        {
            ViewBag.lstEvents = await api.HttpGetSubscriptionEvents(SubId);
            //obrisat
            string username = ((UserViewModel)Session["UserViewModel"]).UserName;
            List<EventListModel> events = await api.HttpGetEvents(username, "dd");
            ViewBag.lstEvents = events;



            Subscription model = new Subscription();
            model.UserName = ((UserViewModel)Session["UserViewModel"]).UserName;
            ViewBag.lstSubscriptions = await api.HttpGetUserSubscriptions(((UserViewModel)(Session["UserViewModel"])).UserName);
            ViewBag.lstSports = await api.HttpGetAllSports();
            ViewBag.lstSubscriptions = await api.HttpGetUserSubscriptions(model.UserName);
            //obrisat
            ViewBag.lstSubscriptions = new List<Subscription>() { new Subscription() { Id = 1, Name = "Sub1" }, new Subscription() { Id = 2, Name = "Sub2" } };
            return View("Home", model);
        }

        // GET: Subscription/Create
        public async Task<ActionResult> Home()
        {
            Subscription model = new Subscription();
            model.UserName = ((UserViewModel)Session["UserViewModel"]).UserName;
            ViewBag.lstSubscriptions = await api.HttpGetUserSubscriptions(((UserViewModel)(Session["UserViewModel"])).UserName);
            ViewBag.lstSports = await api.HttpGetAllSports();
            ViewBag.lstSubscriptions = await api.HttpGetUserSubscriptions(model.UserName);
            ViewBag.lstSubscriptions = new List<Subscription>() { new Subscription() { Id = 1, Name = "Sub1" }, new Subscription() { Id = 2, Name = "Sub2" }};
            return View(model);
        }

        // POST: Subscription/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "UserName,SportId,CityName,Name")] Subscription model)
        {
            string response = await api.HttpCreateSubscription(model);
            if (response.Equals("OK"))
            {
                return RedirectToAction("Home");
            }
            else
            {
                model = new Subscription();
                model.UserName = ((UserViewModel)Session["UserViewModel"]).UserName;
                ViewBag.lstSubscriptions = await api.HttpGetUserSubscriptions(((UserViewModel)(Session["UserViewModel"])).UserName);
                ViewBag.lstSports = await api.HttpGetAllSports();
                ViewBag.lstSubscriptions = await api.HttpGetUserSubscriptions(model.UserName);
                ViewBag.lstSubscriptions = new List<Subscription>() { new Subscription() { Id = 1, Name = "Sub1" }, new Subscription() { Id = 2, Name = "Sub2" } };
                ViewBag.error = response;
                return View("Home", model);
            }
        }

        // GET: Subscription/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Subscription/Edit/5
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

        // GET: Subscription/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Subscription/Delete/5
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
