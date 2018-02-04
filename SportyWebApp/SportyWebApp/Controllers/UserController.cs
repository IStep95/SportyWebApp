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
        API api = new API();
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
            if(!api.InternetConnectionEstablished())
            {
                _userLoginModel.InternetNotAvaiable = true;
                return RedirectToAction("Login", "User", _userLoginModel);
            }

            _userLoginModel.InternetNotAvaiable = false;
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
        public ActionResult Register()
        {
            UserRegisterModel model = new UserRegisterModel();
            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        public async Task<ActionResult> Register([Bind(Include = "FirstName,LastName,Email,City,Password,UserName,ConfirmPassword")] UserRegisterModel user)
        {
            if (!user.EmptyCheck())
            {
                ViewBag.error = "Popunite sve podatke!";
                return View(user);
            }
            if (!user.PasswordCheck())
            {
                ViewBag.error = "Lozinke se ne podudaraju!";
                return View(user);
            }
            string response = await api.HttpCreateUser(user);
            if (response.Equals("OK"))
            {
                UserViewModel userViewModel = await api.HttpGetUser(user.UserName, user.Password);
                Session["UserViewModel"] = userViewModel;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.error = response;
                return View(user);
            }
        }
    }
}
