using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Travel_Assistant.Models;

namespace Travel_Assistant.Services
{
    public class RDatabaseConsumer
    {
        static readonly string BaseUrl = "https://travel-assistant-d3dc7-default-rtdb.europe-west1.firebasedatabase.app/";

        public static async Task<Dictionary<string, Train>> GetAllTrains()
        {
            string url = BaseUrl + "trenuri.json";

            HttpClient http = new HttpClient();

            HttpResponseMessage response = await http.GetAsync(url);

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();

                var json = JsonConvert.DeserializeObject<Dictionary<string, Train>>(result);

                return json;
            }

            return null;
        }

        public static async Task<Dictionary<string, Price>> GetPrices()
        {
            string url = BaseUrl + "Preturi/Clase.json";

            HttpClient http = new HttpClient();

            HttpResponseMessage response = await http.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
            
                var json = JsonConvert.DeserializeObject<Dictionary<string,Price>>(result);
            
                return json;
            }

            return null;
        }

        public static async Task<UniversityBadge> GetBadge(string badgeID, string university)
        {
            string univ = university.Replace(" ", "%20");
            string url = String.Format("{0}Legitimatii/{1}/{2}.json", BaseUrl, univ, badgeID);

            HttpClient http = new HttpClient();

            HttpResponseMessage response = await http.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();

                var json = JsonConvert.DeserializeObject<UniversityBadge>(result);

                return json;
            }

            return null;
        }
    }
}
