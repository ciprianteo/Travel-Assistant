using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Travel_Assistant.Models;
using Travel_Assistant.Services;
using Travel_Assistant.Views;
using Xamarin.Forms;

namespace Travel_Assistant.ViewModels
{
    [QueryProperty(nameof(Arrive), nameof(Arrive))]
    [QueryProperty(nameof(Depart), nameof(Depart))]
    [QueryProperty(nameof(DepartDate), nameof(DepartDate))]
    public class TrainViewModel
    {
        string _statiePlecare;
        string _statieSosire;
        DateTime _departDate { get; set; }
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
        public string DepartDate
        {
            get => _departDate.ToString();
            set { _departDate = DateTime.Parse(Uri.UnescapeDataString(value ?? string.Empty)); }
        }
        public Command<TrainDisplayModel> ItemTapped { get; }
        public ObservableCollection<TrainDisplayModel> Trains { get; set; }

        public TrainViewModel()
        {
            Trains = new ObservableCollection<TrainDisplayModel>();
            ItemTapped = new Command<TrainDisplayModel>(OnItemSelected);
            SearchTrains();
        }

        private async void SearchTrains()
        {
            var trains = await RDatabaseConsumer.GetAllTrains();
            foreach( KeyValuePair<string,Train> t in trains)
            {
                var departureStation = t.Value.Statii[_statiePlecare];
                var arrivalStation = t.Value.Statii[_statieSosire];

                if(departureStation.Km < arrivalStation.Km)
                {
                    Trains.Add(new TrainDisplayModel
                    {
                        ID = t.Key,
                        DepartureCityDepartureTime = t.Value.Statii[_statiePlecare].Plecare,
                        ArrivalCityArrivalTime = t.Value.Statii[_statieSosire].Sosire,
                        Distance = t.Value.Statii[_statieSosire].Km - t.Value.Statii[_statiePlecare].Km
                    });
                }
            }
        }

        private async void OnItemSelected(TrainDisplayModel item)
        {
            if (item == null)
                return;

            var depTime = DateTime.ParseExact(item.DepartureCityDepartureTime, "HH:mm", CultureInfo.InvariantCulture);
            DateTime departure = new DateTime(_departDate.Date.Year, _departDate.Date.Month, _departDate.Date.Day, depTime.Hour, depTime.Minute, 0);

            var arrTime = DateTime.ParseExact(item.ArrivalCityArrivalTime, "HH:mm", CultureInfo.InvariantCulture);
            DateTime arrival = new DateTime(_departDate.Date.Year, _departDate.Date.Month, _departDate.Date.Day, arrTime.Hour, arrTime.Minute, 0);
            
            if(arrTime < depTime)
                arrival.AddDays(1);

            await Shell.Current.GoToAsync($"{nameof(BuyTicketPage)}?TrainID={item.ID}&DepartureStation={ Depart }&ArrivalStation={ Arrive }" +
                $"&DepartureDate={departure}&ArrivalDate={arrival}&Distance={item.Distance}");
        }
    }

    public class TrainDisplayModel
    {
        public string ID { get; set; }
        public string DepartureCityDepartureTime { get; set; }
        public string ArrivalCityArrivalTime { get; set; }
        public int Distance { get; set; }
    }
}
