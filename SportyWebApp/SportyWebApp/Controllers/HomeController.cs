using SportyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportyWebApp.Controllers
{
    public class HomeController : Controller
    {
        UserViewModel _userViewModel;

        
        public ActionResult Index(UserViewModel userViewModel)
        {       
            _userViewModel = (UserViewModel) Session["UserViewModel"];

            //Mock up data danas u gradu
            List<EventViewModel> list = new List<EventViewModel>();
            EventViewModel e1 = new EventViewModel();
            e1.Id = 1;
            e1.MaxPlayers = 10;
            e1.FreePlayers = 2;
            e1.StartTime = new DateTime(2018, 1, 22, 19, 0, 0);
            e1.Location = "Starotrnjanska ulica 48";
            e1.City = new CityViewModel();
            e1.City.Name = "Zagreb";
            e1.Sport = new SportViewModel();
            e1.Sport.Name = "Mali nogomet";

            EventViewModel e2 = new EventViewModel();
            e2.Id = 2;
            e2.MaxPlayers = 12;
            e2.FreePlayers = 4;
            e2.StartTime = new DateTime(2018, 1, 22, 18, 0, 0);
            e2.Location = "Maksimirska 101";
            e2.City = new CityViewModel();
            e2.City.Name = "Zagreb";
            e2.Sport = new SportViewModel();
            e2.Sport.Name = "Košarka";

            EventViewModel e3 = new EventViewModel();
            e3.Id = 2;
            e3.MaxPlayers = 12;
            e3.FreePlayers = 4;
            e3.StartTime = new DateTime(2018, 1, 22, 18, 0, 0);
            e3.Location = "Metalčeva 51";
            e3.City = new CityViewModel();
            e3.City.Name = "Zagreb";
            e3.Sport = new SportViewModel();
            e3.Sport.Name = "Tenis";

            list.Add(e1);
            list.Add(e2);
            list.Add(e3);
            

            ViewBag.EventsToday = list;
            return View();
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