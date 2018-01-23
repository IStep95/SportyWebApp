using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportyWebApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SportyWebApp.WebAPI
{
    public class API
    {
        string _apiUrl = "http://sportfinderapi.azurewebsites.net";
        HttpClient _client;
        public API()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_apiUrl);
        }

        public async Task<UserViewModel> HttpGetUser(int id)
        {
            UserViewModel userViewModel = new UserViewModel();
            _client.DefaultRequestHeaders.Clear();
            HttpResponseMessage response = await _client.GetAsync("api/users/getbyid/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                userViewModel = JsonConvert.DeserializeObject<UserViewModel>(data);
            }
            return userViewModel;
        }

        public async Task<UserViewModel> HttpGetUser(string username, string password)
        {
            UserViewModel userViewModel = new UserViewModel();
            JObject jsonObject = new JObject();
            jsonObject.Add("UserName", username);
            jsonObject.Add("Password", password);

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");


            var response = await _client.PostAsync("/api/users/login", content);

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    userViewModel = JsonConvert.DeserializeObject<UserViewModel>(data);
                    return userViewModel;
                }
            }
            return null;
        }

<<<<<<< HEAD
        public async Task<string> HttpCreateUser(UserRegisterModel user)
        {
            JObject jsonObject = new JObject();
            jsonObject.Add("UserName", user.UserName);
            jsonObject.Add("Password", user.Password);
            jsonObject.Add("FirstName", user.FirstName);
            jsonObject.Add("LastName", user.LastName);
            jsonObject.Add("Email", user.Email);
            jsonObject.Add("CityName", user.City);

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");


            var response = await _client.PostAsync("/api/Users/Register", content);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "OK";
                }
            }
            JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            return responseObject.GetValue("Message").ToString();
        }

        public async Task<List<EventViewModel>> HttpGetTodayEventsByCityId(int id)
=======
        public async Task<List<EventViewModel>> HttpGetTodayEventsByCityId(string username)
>>>>>>> b18c3639291fe2b29aa007b62ae260988ecfcf4d
        {
            List<EventViewModel> todayEvents = new List<EventViewModel>();

            _client.DefaultRequestHeaders.Clear();
            DateTime date = DateTime.Now;
            string dateString = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string queryString = "?username=" + username + "&date=" + dateString;
            HttpResponseMessage response = await _client.GetAsync("api/events/getbycity" + queryString);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                todayEvents = JsonConvert.DeserializeObject<List<EventViewModel>>(data);

            }
            return todayEvents;
        }

        //public async Task<List<EventViewModel>> HttpGetEvents(string username)
        //{
        //    List<EventViewModel> lst = new List<EventViewModel>();
        //    var response = await _client.GetAsync("api");
        //    if (response.IsSuccessStatusCode)
        //    {
        //        JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
        //    }
        //    return lst;
        //}

        public async Task<string> HttpCreateEvent(EventCreateModel model)
        {
            UserViewModel _userViewModel = new UserViewModel();
            JObject jsonObject = new JObject();
            jsonObject.Add("SportId", model.SportId);
            jsonObject.Add("City", model.City);
            jsonObject.Add("UserName", model.UserName);
            jsonObject.Add("MaxPlayers", model.MaxPlayers);
            jsonObject.Add("FreePlayers", model.FreePlayers);
            jsonObject.Add("StartTime", model.Date);
            jsonObject.Add("Location", model.Location);

            var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");


            var response = await _client.PostAsync("/api/Events/Create", content);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return "OK";
                }
            }
            return "Error";
        }
    }
}