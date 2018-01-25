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
    public class ProfileController : Controller
    {
        UserViewModel _userViewModel;
        API api = new API();

        // GET: Profile/Edit
        [HttpGet]
        public ActionResult Edit()
        {
            _userViewModel = (UserViewModel)Session["UserViewModel"];
            if (_userViewModel == null) return RedirectToAction("Login", "User");

            ViewBag.FirstName = _userViewModel.FirstName;
            ViewBag.LastName = _userViewModel.LastName;
            ViewBag.Username = _userViewModel.UserName;
            ViewBag.Email = _userViewModel.Email;
            ViewBag.CityName = _userViewModel.CityName;
            return View(_userViewModel);
        }

        // POST: Profile/Save
        public async Task<ActionResult> Save(string firstName, string lastName, string username, string email, string cityName)
        {
            
            var x = await api.HttpGetAllSports();

            return RedirectToAction("Edit", "Profile");
        }
    }
}