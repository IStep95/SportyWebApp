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
            List<EventViewModel> todayEvents = await _api.HttpGetTodayEventsByCityId(_userViewModel.UserName);
            foreach (var entry in todayEvents)
            {
                var sportName = entry.SportName;
                if (!String.IsNullOrEmpty(sportName))
                {
                    sportName = sportName.First().ToString().ToUpper() + sportName.Substring(1);
                    entry.SportName = sportName;
                }
            }  
            ViewBag.EventsToday = todayEvents;
            ViewBag.CurrentPage = "HomePage";
            return View(_userViewModel);
        }
    }
}