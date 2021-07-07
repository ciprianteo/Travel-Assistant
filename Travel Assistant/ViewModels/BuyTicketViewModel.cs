using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Travel_Assistant.Views;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    public class BuyTicketViewModel
    {
        public List<string> StationsList { get; set; }
        public int SourceIdx { get; set; }
        public int DestIdx { get; set; }
        public Command SearchTrainCommand { get; set; }

        public BuyTicketViewModel()
        {
            SearchTrainCommand = new Command(OnClickSearchTrain);
            SourceIdx = -1;
            DestIdx = -1;

            var assembly = typeof(BuyTicketPage).Assembly;
            Stream stream = assembly.GetManifestResourceStream("Travel_Assistant.Resources.statii.json");

            using (var reader = new System.IO.StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                StationsList = JsonConvert.DeserializeObject<List<string>>(json);
            }
        }

        private async void OnClickSearchTrain()
        {
            await Shell.Current.GoToAsync($"{nameof(TrainsPage)}?Depart={ StationsList[SourceIdx] }&Arrive={ StationsList[DestIdx] }");
        }
    }
}
