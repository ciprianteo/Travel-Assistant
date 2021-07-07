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
    }
}
