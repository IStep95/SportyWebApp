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
    public class HomeController : Controller
    {
        API _api = new API();
        UserViewModel _userViewModel;

        
        public async Task<ActionResult> Index(UserViewModel userViewModel)
        {       
            _userViewModel = (UserViewModel) Session["UserViewModel"];

            List<EventViewModel> todayEvents = await _api.HttpGetTodayEventsByCityId(_userViewModel.CityId);
            
            ViewBag.EventsToday = todayEvents;
            return View(_userViewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}