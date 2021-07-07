using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    [QueryProperty(nameof(Arrive), "Arrive")]
    [QueryProperty(nameof(Depart), "Depart")]
    public class TrainViewModel
    {
        string _statiePlecare;
        string _statieSosire;
        public string Arrive
        {
            get => _statieSosire;
            set { _statieSosire = Uri.UnescapeDataString(value ?? string.Empty); }
        }
        public string Depart
        {
            get => _statiePlecare;
            set { _statiePlecare = Uri.UnescapeDataString(value ?? string.Empty); }
        }
       public ObservableCollection<TrainDisplayModel> Trains { get; set; }

        public TrainViewModel()
        {
            Trains = new ObservableCollection<TrainDisplayModel>();
            SearchTrains();
        }

        private async void SearchTrains()
        {
            var trains = await RDatabaseConsumer.GetAllTrains();
            foreach( KeyValuePair<string,Train> t in trains)
            {
                Trains.Add( new TrainDisplayModel {
                    ID = t.Key,
                    DepartureCityDepartureTime = t.Value.Statii[_statiePlecare].Plecare,
                    ArrivalCityArrivalTime = t.Value.Statii[_statieSosire].Sosire
                });
            }
        }
    }

    public class TrainDisplayModel
    {
        public string ID { get; set; }
        public string DepartureCityDepartureTime { get; set; }
        public string ArrivalCityArrivalTime { get; set; }
    }
}
