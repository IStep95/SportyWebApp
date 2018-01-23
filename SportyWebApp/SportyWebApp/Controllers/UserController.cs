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
        UserViewModel _userViewModel;
        UserLoginModel _userLoginModel = new UserLoginModel();
        UserRegisterModel _userRegisterModel = new UserRegisterModel();


        // GET: User/Login
        public ActionResult Login(UserLoginModel userLoginModel)
        {
            Session.Abandon();
            return View(userLoginModel);
        }

        // POST: User/Submit
        [HttpPost]
        public async Task<ActionResult> Submit(string username, string password)
        {
            API api = new API();
            _userViewModel = await api.HttpGetUser(username, password);
           
            if (_userViewModel != null)
            {
                _userLoginModel.UserNotExist = false;
                Session["UserViewModel"] = _userViewModel;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _userLoginModel.UserNotExist = true;
                return RedirectToAction("Login", "User", _userLoginModel);
            }
        }
        
        // GET: User/Register
        [HttpGet]
        public async Task<ActionResult> Register()
        {
            return await Create(_userRegisterModel);
        }

        // POST: User/Create
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,Email,City,Password,UserName")] UserRegisterModel user)
        {
            API api = new API();
            string response = await api.HttpCreateUser(user);
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
        }
    }
}
