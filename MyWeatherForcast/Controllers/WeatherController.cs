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
        public ActionResult Error()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult WeatherInfo()
        {
            //var t = await GetWeather("London", 1);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> WeatherInfo(WeatherInformation weatherInformation)
        {
            var location = weatherInformation.Location;
            var numberOfDays = weatherInformation.NumberOfDays;
            if (string.IsNullOrEmpty(location) || numberOfDays <= 0)
            {
                return View("Error");
            }
            else
            {
                var t = await GetWeather(location, (numberOfDays > 0 ? numberOfDays : 1));
                if(t.Weath == null)
                {
                    return View("Error");
                }
                else
                {
                    t.Location = location;
                    t.NumberOfDays = numberOfDays;
                    return View(t);
                }
            }
            //return View();
        }

        public async Task<WeatherInformation> GetWeather(string loc, int numberOfDays)
        {
            try
            {
                WeatherInformation weatherList = new WeatherInformation();

                if (!string.IsNullOrEmpty(loc) && numberOfDays > 0)
                {
                    using (var client = new HttpClient())
                    {
                        var subUrl = Properties.Settings.Default.subUrl + loc + "&cnt=" + (numberOfDays *8) + "&appid=" + Properties.Settings.Default.apiKey + "&units=metric";
                                                                 
                        client.BaseAddress = new Uri(Properties.Settings.Default.baseUrl);
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage Res = await client.GetAsync(subUrl);
                        if (Res.IsSuccessStatusCode)
                        {
                            string result = Res.Content.ReadAsStringAsync().Result;
                            var wets = JsonConvert.DeserializeObject<WeatherInformation>(result);

                            var inputDays = DateTime.UtcNow.Date.AddDays(numberOfDays);
                            var filteredWeatherList = wets.Weath.DistinctBy(c => c.dateformatted).Where(x => x.dateformatted < inputDays).ToList();
                            
                            weatherList.Weath = filteredWeatherList;

                            return weatherList;
                        }
                        else
                        {
                            return weatherList;
                        }
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