using Microsoft.Ajax.Utilities;
using MyWeatherForecast.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyWeatherForecast.Controllers
{
    public class WeatherController : Controller
    {
        public string Location { get; set; }
        public int NumberOfDays { get; set; }
        // GET: Weather
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //string Baseurl = "https://api.openweathermap.org/data/2.5/";
        //string weather = "weather?q=London&appid={API key}";
        //string forecast = "forecast?lat={lat}&lon={lon}&appid={API key}";
        //https://api.openweathermap.org/data/2.5/forecast?q=London,GB&&appid=0037a205884c254c3fe1eb531093170a
        //https://api.openweathermap.org/data/2.5/forecast?q=London&&appid=0037a205884c254c3fe1eb531093170a&cnt=1
        //api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={API key}
        [HttpGet]
        public async Task<ActionResult> WeatherInfo()
        {
            //WeatherInformation wets = new WeatherInformation();
            var t = await Rer("London", 1);
            //List<WeatherInformation> weatherInfo = new List<WeatherInformation>();
            return View(t);
        }
        [HttpPost]
        public async Task<ActionResult> WeatherInfo(WeatherInformation weatherInformation)
        {
            var location = weatherInformation.Location;
            var numberOfDays = weatherInformation.NumberOfDays;
            //WeatherInformation wets = new WeatherInformation();
            var t = await Rer(location, (numberOfDays > 0 ? numberOfDays : 1));
            //List<WeatherInformation> weatherInfo = new List<WeatherInformation>();
            return View(t);
        }

        public async Task<WeatherInformation> Rer(string loc, int numberOfDays)
        {
            try
            {
                WeatherInformation weatherList = new WeatherInformation();

                if (!string.IsNullOrEmpty(loc) && numberOfDays > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var subUrl = Properties.Settings.Default.subUrl + loc + "&cnt=" + (numberOfDays *8) + "&appid=" + Properties.Settings.Default.apiKey + "&units=metric";
                        //Passing service base url                                         
                        client.BaseAddress = new Uri(Properties.Settings.Default.baseUrl);
                        client.DefaultRequestHeaders.Clear();
                        //Define request data format
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //Sending request to find weather forecast using HttpClient
                        //HttpResponseMessage Res = await client.GetAsync("forecast?q=London&&appid=0037a205884c254c3fe1eb531093170a&cnt=5&units=metric");
                        HttpResponseMessage Res = await client.GetAsync(subUrl);
                        //Checking the response is successful or not which is sent using HttpClient
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api
                            string result = Res.Content.ReadAsStringAsync().Result;
                            //var js = JsonConvert.SerializeObject(result);
                            //Deserializing the response recieved from web api
                            var wets = JsonConvert.DeserializeObject<WeatherInformation>(result);
                            //List<WeatherData> wet = wets.Weath.DistinctBy(c=> c.dt).ToList();

                            var inputDays = DateTime.UtcNow.Date.AddDays(numberOfDays);
                            var filteredWeatherList = wets.Weath.DistinctBy(c => c.dateformatted).Where(x => x.dateformatted < inputDays).ToList();
                            
                            weatherList.Weath = filteredWeatherList;
                            //eo.Weath.Add();


                            //var test = JsonConvert.SerializeObject(result);
                            //weatherInfo.Name = test["name"];1
                            //return View(weatherList);
                            
                            //RedirectToAction("WeatherInfo", weatherList);
                        }
                        return weatherList;
                        //returning the weather list to view
                    }
                }
                return weatherList;
            }
            catch (Exception e)
            {
                return WeatherInformation();
            }
        }

        private WeatherInformation WeatherInformation()
        {
            throw new NotImplementedException();
        }
    }
}