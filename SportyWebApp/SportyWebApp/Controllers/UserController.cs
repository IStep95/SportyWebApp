using Newtonsoft.Json.Linq;
using SportyWebApp.Models;
using SportyWebApp.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SportyWebApp.Controllers
{
    public class UserController : Controller
    {
        private static readonly HttpClient client = new HttpClient();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View(new UserRegisterModel());
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "FirstName,LastName,Email,City,Password,UserName")] UserRegisterModel user)
        {
            API api = new API();
            string response = api.HttpCreateUser(user).ToString();
            if (response.Equals("OK"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.poruka = response;
                user.Password = "";
                return View(user);
            }
            return View();
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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
