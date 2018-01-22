using SportyWebApp.Models;
using SportyWebApp.WebAPI;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportyWebApp.Controllers
{
    public class LoginController : Controller
    {
        API _api = new API();
        LoginViewModel _loginViewModel = new LoginViewModel();
        UserViewModel _userViewModel;


        // GET: Login
        public ActionResult Index(LoginViewModel loginViewModel)
        {
            Session.Abandon();
            return View(loginViewModel);
        }


        // POST: Submit
        [HttpPost]
        public async Task<ActionResult> Submit(string username, string password)
        {
            UserViewModel userViewModel = await _api.HttpGetUser(username, password);
            _userViewModel = userViewModel;

            if (userViewModel != null)
            {
                _loginViewModel.UserNotExist = false;
                Session["UserViewModel"] = _userViewModel;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _loginViewModel.UserNotExist = true;
                return RedirectToAction("Index", "Login", _loginViewModel);
            }
        }
    }
}