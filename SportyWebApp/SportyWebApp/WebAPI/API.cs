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

        public async Task<List<EventListModel>> HttpGetTodayEvents(string username)
        {
            List<EventListModel> todayEvents = new List<EventListModel>();
            _client.DefaultRequestHeaders.Clear();
            DateTime date = DateTime.Now;
            string dateString = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string queryString = "?username=" + username + "&date=" + dateString;
            HttpResponseMessage response = await _client.GetAsync("api/Events/GetByCity" + queryString);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                todayEvents = JsonConvert.DeserializeObject<List<EventListModel>>(data);

            }
            return todayEvents;
        }


        public async Task<string> HttpCreateEvent(EventCreateModel model)
        {
            UserViewModel _userViewModel = new UserViewModel();
            JObject jsonObject = new JObject();
            jsonObject.Add("SportId", model.SportId);
            jsonObject.Add("CityName", model.City);
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
            JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
            return responseObject.GetValue("Message").ToString();
        }

        public async Task<EventDetailsModel> HttpGetEvent(int id)
        {
            EventDetailsModel model = new EventDetailsModel();
            List<UserEventModel> lst = new List<UserEventModel>();
            var response = await _client.GetAsync("/api/Events/GetEvent" + "?id=" + id);
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject obj = JObject.Parse(data);
                    JToken participants = obj.SelectToken("Participants");
                    foreach (var user in participants)
                    {
                        string json = JsonConvert.SerializeObject(user);
                        lst.Add(JsonConvert.DeserializeObject<UserEventModel>(json));
                    }
                    model = JsonConvert.DeserializeObject<EventDetailsModel>(data);
                }
            }
            model.lstUsers = lst;
            return model;
        }


	    public async Task<List<EventListModel>> HttpGetEvents(string username, string time)
	    {
	        List<EventListModel> lst = new List<EventListModel>();
	        var response = await _client.GetAsync("/api/Events/GetUserEvents" + "?username=" + username);
	        if (response.IsSuccessStatusCode)
	        {
	            if (response.StatusCode == System.Net.HttpStatusCode.OK)
	            {
	                string data = await response.Content.ReadAsStringAsync();
	                JObject obj = JObject.Parse(data);
	                JToken events;
	                if (time.Equals("futureevents"))
	                {
	                    events = obj.SelectToken("FutureEvents");
	                }
	                else
	                {
	                    events = obj.SelectToken("PastEvents");
	                }
	                foreach (var item in events)
	                {
	                    string json = JsonConvert.SerializeObject(item);
	                    lst.Add(JsonConvert.DeserializeObject<EventListModel>(json));
	                }
	            }
	        }
	        return lst;
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

	    public async Task<List<EventListModel>> HttpFindEvents(string sportId, string date, string cityName, string freePlayers)
	    {
	        List<EventListModel> searchEvents = new List<EventListModel>();
	        _client.DefaultRequestHeaders.Clear();
	        string queryString;
	        DateTime dateAPIFormat;
	        if (DateTime.TryParse(date, out dateAPIFormat))
	        {
	            string dateString = dateAPIFormat.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
	            queryString = "?sportId=" + sportId + "&date=" + dateString + "&cityName=" + cityName + "&freePlayers=" + freePlayers;
	        }
	        else
	        {
	            queryString = "?sportId=" + sportId + "&date=" + date + "&cityName=" + cityName + "&freePlayers=" + freePlayers;
	        }
	        
	        HttpResponseMessage response = await _client.GetAsync("api/Events/FindEvents" + queryString);
	        if(response.IsSuccessStatusCode)
	        {
	            var data = await response.Content.ReadAsStringAsync();
	            searchEvents = JsonConvert.DeserializeObject<List<EventListModel>>(data);
	        }
	        return searchEvents;
	    }
	}
}