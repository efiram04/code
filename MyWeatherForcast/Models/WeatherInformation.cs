using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeatherForecast.Models
{
    public class WeatherInformation : UserInput
    {
        [JsonProperty("list")]
        public List<WeatherData> Weath { get; set; }
        
    }
    public class WeatherData
    {
        [JsonProperty("dt_txt")]
        public DateTime DateTimeStamp { get; set; }
        [JsonProperty("weather")]
        public List<DayWeather> DayWeatherInfo { get; set; }
        [JsonProperty("main")]
        public TempDetails DayTemperatureInfo { get; set; }
        public DateTime dateformatted { 
            get 
            {
                DateTime.TryParse(DateTimeStamp.ToShortDateString(), out DateTime dtformatted);
                return dtformatted; 
            } 
        }
        public string TransformedDate { get { return ToOrdinal(); } }
        public string ToOrdinal()
        {
            var dtStamp = DateTimeStamp;
            var dtNow = DateTime.UtcNow;
            string dayOfWeek = dtStamp.DayOfWeek.ToString();
            string transformedDate;
            if (DateTime.UtcNow.DayOfWeek.ToString() == dayOfWeek)
            {
                dayOfWeek = "Today";
            }

            int day = dtStamp.Day;
            string extension = "th";

            int last_digits = day % 100;

            if (last_digits < 11 || last_digits > 13)
            {
                switch (last_digits % 10)
                {
                    case 1:
                        extension = "st";
                        break;
                    case 2:
                        extension = "nd";
                        break;
                    case 3:
                        extension = "rd";
                        break;
                }
            }

            transformedDate = dayOfWeek + ',' + day.ToString() + extension;

            return transformedDate;
        }

    }
    public class DayWeather
    {
        public int id { get; set; }
        public string description { get; set; }
        public string icon { get { return wIcon(); } }
        public string main { get; set; }
        public string wIcon()
        {
            string weathericon = "01d.svg";
            var weatherID = id;

            if (weatherID == 200 && weatherID <= 232)
            {
                return weathericon = "11d.svg";
            }
            else if ((weatherID == 300 && weatherID <= 321) || (weatherID == 520 && weatherID == 531))
            {
                return weathericon = "09d.svg";
            }
            else if (weatherID == 500)
            {
                if (weatherID == 500 && weatherID <=504)
                {
                    return weathericon = "10d.svg";
                }
                else if (weatherID == 511 || weatherID == 600 && weatherID <= 621)
                {
                    return weathericon = "13d.svg";
                }
            }
            else if (weatherID == 701 && weatherID <= 781)
            {
                return weathericon = "50d.svg";
            }
            else if (weatherID > 800)
            {
                if (weatherID == 801)
                {
                    return weathericon = "02d.svg";
                }
                else if (weatherID == 802)
                {
                    return weathericon = "03d.svg";
                }
                else if (weatherID == 803 || weatherID == 804)
                {
                    return weathericon = "04d.svg";
                }
            }
            else
            {
                return weathericon;
            }
            return weathericon;
        }
    }
    public class TempDetails
    {
        [JsonProperty("temp")]
        public double CTemperature { get; set; }
        public double FTemperature { get { return Farenheit(); } }
        public double Farenheit()
        {
            var celciusTemp = CTemperature;
            var farenheit = (celciusTemp * 9 / 5) + 32;
            return farenheit;
        }

        //(0°C × 9/5) + 32 = 32°F

    }
}