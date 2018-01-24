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
            HttpResponseMessage response = await _client.GetAsync("api/Users/GetById/" + id);

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


            var response = await _client.PostAsync("/api/Users/Login", content);

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

        public async Task<List<EventViewModel>> HttpGetTodayEvents(string username)
        {
            List<EventViewModel> todayEvents = new List<EventViewModel>();

            _client.DefaultRequestHeaders.Clear();
            DateTime date = DateTime.Now;
            string dateString = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string queryString = "?username=" + username + "&date=" + dateString;
            HttpResponseMessage response = await _client.GetAsync("api/Events/GetByCity" + queryString);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                todayEvents = JsonConvert.DeserializeObject<List<EventViewModel>>(data);

            }
            return todayEvents;
        }

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
        public async Task<List<SportViewModel>> HttpGetAllSports()
        {
            List<SportViewModel> allSports = new List<SportViewModel>();
            _client.DefaultRequestHeaders.Clear();
            HttpResponseMessage response = await _client.GetAsync("api/Events/GetAllSports");
 
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                allSports = JsonConvert.DeserializeObject<List<SportViewModel>>(data);      
            }
            return allSports;
        }

    }
}